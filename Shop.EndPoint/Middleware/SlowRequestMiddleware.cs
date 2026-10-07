using System.Diagnostics;

namespace Shop.EndPoint.Middleware
{
    public class SlowRequestMiddleware
    {
        //in MiddleWare baraye ine ke age api bishtar az 2 sanie tool keshid warning sabt kone dar log
        // ke in modat ra az appsetting mikhone
        private readonly RequestDelegate _next;
        private readonly ILogger<SlowRequestMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public SlowRequestMiddleware(
            RequestDelegate next,
            ILogger<SlowRequestMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var threshold = _configuration
                .GetValue<int>("LoggingSettings:SlowRequestThresholdMs");

            if (stopwatch.ElapsedMilliseconds > threshold)
            {
                _logger.LogWarning(
                    "Slow request detected. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}, ElapsedMs: {ElapsedMs}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
