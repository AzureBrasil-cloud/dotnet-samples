namespace OpenApi.WebApi.Extensions;

public static class ApiKeyMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder app, string apiKeyHeaderName = "x-api-key")
    {
        return app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/openapi"))
            {
                await next();
                return;
            }

            var apiKey = context.Request.Headers[apiKeyHeaderName].FirstOrDefault();
            var configApiKey = app.ApplicationServices.GetService(typeof(IConfiguration)) is IConfiguration config
                ? config["ApiKey"]
                : null;

            if (string.IsNullOrEmpty(configApiKey) || apiKey != configApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next();
        });
    }
}