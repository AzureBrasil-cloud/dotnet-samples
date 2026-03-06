using Microsoft.OpenApi;

namespace OpenApi.WebApi.Extensions;

public static class OpenApiServiceExtensions
{
    public static IHostApplicationBuilder AddCustomOpenApiWithApiKey(
        this IHostApplicationBuilder builder,
        string apiKeyHeaderName = "x-api-key")
    {
        if (builder.Environment.IsProduction())
            return builder;

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();

                document.Components.SecuritySchemes ??=
                    new Dictionary<string, IOpenApiSecurityScheme>();

                const string schemeName = "ApiKey";

                document.Components.SecuritySchemes[schemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = apiKeyHeaderName,
                    In = ParameterLocation.Header
                };

                document.Security ??= new List<OpenApiSecurityRequirement>();

                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(schemeName)] = []
                });

                return Task.CompletedTask;
            });
        });

        return builder;
    }
}