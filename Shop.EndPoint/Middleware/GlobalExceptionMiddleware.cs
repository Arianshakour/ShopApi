using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace Shop.EndPoint.Middleware
{
    public class GlobalExceptionMiddleware
    {
        //In Middleware baraye ine ke har jaei Exception rokh dad kamel detailesho darim
        //va niaz nis dg _logger.LogError(ex.Message); bezarim
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //in khate paein mige har logi ke vase har request tolid beshe
            //yek property be esm TraceId dare ke betoonim vase ye Request id yekta
            //dar controller o redis o service o ... dashte bashim
            using (LogContext.PushProperty("TraceId",context.TraceIdentifier))
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Unhandled exception. Method: {Method}, Path: {Path}",
                        context.Request.Method,
                        context.Request.Path);

                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;

                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        message = "خطایی در سرور رخ داده است."
                    };

                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(response));
                }
            }
        }
    }
}
