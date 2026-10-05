using Serilog.Context;

namespace Shop.EndPoint.Middleware
{
    public class UserContextLoggingMiddleware
    {
        //In Middleware baraye ine ke UserId karbar ra ham dashte bashim
        // va bayad karbar ehraz hoviat karde bashe
        // pas jaye in Middleware bad Middleware authentication hast
        private readonly RequestDelegate _next;

        public UserContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirst("Id")?.Value;

            using (LogContext.PushProperty(
                "UserId",
                userId ?? "Anonymous"))
            using (LogContext.PushProperty(
                "TraceId",
                context.TraceIdentifier))
            {
                await _next(context);
            }
        }
    }
}
