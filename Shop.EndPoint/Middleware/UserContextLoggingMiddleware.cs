using Serilog.Context;

namespace Shop.EndPoint.Middleware
{
    public class UserContextLoggingMiddleware
    {
        //In Middleware baraye ine ke UserId karbar ra ham dashte bashim
        // va bayad karbar ehraz hoviat karde bashe
        // pas jaye in Middleware bad Middleware authentication hast
        //khoroji log ba in mishe
        //{
        //    "TraceId": "0HNP...",
        //    "UserId": "1",
        //    "RequestPath": "/api/product/7",
        //    "RequestMethod": "GET"
        //}
        private readonly RequestDelegate _next;

        public UserContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirst("Id")?.Value;

            //ba in ip karbar migirim albate age LoadBalancer ya proxy nadashte bashim ip karbar dorost miad
            //age dashte bashim ip ona miadesh
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            using (LogContext.PushProperty(
                "UserId",
                userId ?? "Anonymous"))
            using (LogContext.PushProperty(
                "TraceId",
                context.TraceIdentifier))
            using (LogContext.PushProperty(
                "RequestPath",
                context.Request.Path))
            using (LogContext.PushProperty(
                "RequestMethod",
                context.Request.Method))
            using (LogContext.PushProperty(
                "IpAddress",
                ipAddress ?? "Unknown"))
            {
                await _next(context);
            }
        }
    }
}
