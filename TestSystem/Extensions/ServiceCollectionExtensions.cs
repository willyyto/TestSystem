using TestSystem.Middleware;

namespace TestSystem.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add API response middleware to the application
    /// </summary>
    public static IApplicationBuilder UseApiResponseMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiResponseMiddleware>();
    }
}