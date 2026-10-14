using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.Services.Implementation;
using Shop.Application.Services.Interfaces;
using Shop.EndPoint.Security.Handlers;
using Shop.EndPoint.Security.Policy;
using Shop.Infrastructure.Context;
using Shop.Infrastructure.ElasticSearch;
using Shop.Infrastructure.Configuration;
using Shop.Infrastructure.Repository.Implementation;
using Shop.Infrastructure.Repository.Interfaces;
using System.Text;
using StackExchange.Redis;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using Serilog;
using Shop.EndPoint.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


//for Serilog download
//Serilog.AspNetCore
//Serilog.Sinks.Console
//Serilog.Sinks.File

//Tanzimate Log
//Inja bood bordim dar appsetting chon vase mode haye develop o production onja bashe behtare


var builder = WebApplication.CreateBuilder(args);

//Tanzimate SeriLog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//ba in tanzimat b swagger jwt mifresti va niazi be postman nadari
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<ShopContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ShopContext")));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

//Tanzimate Origin yani az kodom front ha mitoonan be ma darkhast bezanan
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("DefaultPolicy", policy =>
//    {
//        policy
//            .WithOrigins(
//                "http://localhost:3000",
//                "https://admin.myapp.com"
//            )
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

builder.Services.AddScoped<IElasticProductService, ElasticProductService>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

//tanzimate ElasticSearch
//yani maqadir appsetting ro beriz to class ElasticSearchSettings
builder.Services.Configure<ElasticSearchSettings>(
    builder.Configuration.GetSection("ElasticSearch"));
//connection be ElasticSearch
builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ElasticSearchSettings>>().Value;

    var clientSettings = new ElasticsearchClientSettings(new Uri(settings.Url))
        .Authentication(new BasicAuthentication(settings.Username, settings.Password))
        .DefaultIndex(settings.DefaultIndex);

    return new ElasticsearchClient(clientSettings);
});

//tanzimat marboot be JWTBearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer(option =>
{
    option.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
    };
});

//tanzimat marboot be Redis
//hatman chek kon Memurai dar Services windows Running bashe
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("Redis");

    return ConnectionMultiplexer.Connect(connectionString!);
});

//Rate Limit
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;//5 darkhast mojaz ast
        limiterOptions.Window = TimeSpan.FromMinutes(1);//dar baze 1 min
        limiterOptions.QueueLimit = 0;//dar khast ezafe montazer nemimanad
        limiterOptions.AutoReplenishment = true;
    });

    //mahdodiat baraye har karbar
    options.AddPolicy("PerUser", httpContext =>
    {
        var claims = httpContext.User.Claims
        .Select(c => new { c.Type, c.Value })
        .ToList();

        var userId = httpContext.User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? httpContext.User.FindFirst("Id")?.Value;

        var partitionKey = userId
            ?? httpContext.Connection.RemoteIpAddress?.ToString()
            ?? "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

    options.AddConcurrencyLimiter("Concurrency", limiterOptions =>//baraye Darkhasthaye toolani mese gozaresh giri
    {
        limiterOptions.PermitLimit = 2;//faqat 2 ta run mishe hamzaman
        limiterOptions.QueueLimit = 0;//chizi to saf nemimoone
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//baraye Origin
//app.UseCors("DefaultPolicy");

//Middleware Global Log
app.UseMiddleware<GlobalExceptionMiddleware>();

//SeriLog
app.UseSerilogRequestLogging();

app.UseAuthentication();

//hatman bayad ehraz hoviat beshe ta UserId dashte bashe bad bere dar Middleware zir
app.UseMiddleware<UserContextLoggingMiddleware>();

//warning kardan api haye kond
app.UseMiddleware<SlowRequestMiddleware>();

//Baraye RateLimit va Hatman bade Authentication bashe
app.UseRateLimiter();


app.UseAuthorization();

app.MapControllers();

app.Run();
