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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
    };
});

//tanzimat marboot be Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("Redis");

    return ConnectionMultiplexer.Connect(connectionString!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
