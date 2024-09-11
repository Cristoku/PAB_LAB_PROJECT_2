using WeatherAPI.Persistence;

namespace WeatherAPI
{
    public class LoggingMiddleware(RequestDelegate next)
    {
        public Task Invoke(HttpContext httpContext, ApplicationDbContext dbContext, ILogger<LoggingMiddleware> logger)
        {
            var role = httpContext.User.Claims.FirstOrDefault()?.Value;
            var method = httpContext.Request.Method;
            var path = httpContext.Request.Path;

            logger.LogInformation($"User in role {role} requested {method} on {path}");

            return next(httpContext);
        }
    }
}
