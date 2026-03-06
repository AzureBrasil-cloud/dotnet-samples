namespace OpenApi.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorizationAndCors(this IServiceCollection services, string corsPolicyName = "AllowAll")
    {
        services.AddAuthorization();
        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}

