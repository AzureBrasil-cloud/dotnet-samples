namespace OpenApi.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureCustomMiddlewareAndEndpoints(this WebApplicationBuilder builder, string corsPolicyName = "AllowAll")
    {
        builder.Services.AddAuthorizationAndCors(corsPolicyName);
        builder.Services.AddControllers();
        builder.AddCustomOpenApiWithApiKey();

        return builder;
    }
    
    public static WebApplication UseCustomMiddlewareAndEndpoints(this WebApplication app, string apiKeyHeaderName = "x-api-key")
    {
        app.UseHttpsRedirection();
        app.UseCorsPolicy();
        app.UseRouting();
        app.UseAuthorization();
        app.UseApiKeyValidation(apiKeyHeaderName);
        app.MapOpenApiIfNotProduction();
        app.MapControllers();

        return app;
    }
    
    private static WebApplication UseCorsPolicy(this WebApplication app, string policyName = "AllowAll")
    {
        app.UseCors(policyName);
        return app;
    }
}