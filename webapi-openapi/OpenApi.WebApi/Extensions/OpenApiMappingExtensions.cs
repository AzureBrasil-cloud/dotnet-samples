namespace OpenApi.WebApi.Extensions;

public static class OpenApiMappingExtensions
{
    public static IEndpointRouteBuilder MapOpenApiIfNotProduction(this WebApplication app)
    {
        if (!app.Environment.IsProduction())
        {
            app.MapOpenApi();
        }

        return app;
    }
}