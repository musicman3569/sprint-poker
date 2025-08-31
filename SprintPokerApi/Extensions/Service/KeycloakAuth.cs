using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace SprintPokerApi.Extensions.Service;

/// <summary>
/// Provides extension methods for configuring Keycloak JWT authentication in ASP.NET Core applications.
/// </summary>
public static class KeycloakAuth
{
    /// <summary>
    /// Configures JWT authentication with Keycloak, including CORS and authorization policies.
    /// This approach to integrating with OAuth via Keycloak was taken from
    /// https://stackoverflow.com/questions/77084743/secure-asp-net-core-rest-api-with-keycloak
    /// </summary>
    /// <param name="services">The IServiceCollection to add the authentication services to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddKeycloakJwtAuth(this IServiceCollection services)
    {
        services
            .AddAuthentication()
            .AddJwtBearer(options => 
                AddKeycloakJwtBearer(options, services.IsDevelopment())
            );

        services
            .AddKeycloakCors()
            .AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    // Required Realm Roles from Keycloak
                    .RequireClaim("groups", "poker_admin", "poker_user")
                    .Build();
            });
        
        return services;
    }

    /// <summary>
    /// Configures CORS policies for the Keycloak-protected application.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the CORS services to.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    private static IServiceCollection AddKeycloakCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowClientApp", policy =>
            {
                policy
                    .WithOrigins(
                        Environment.GetEnvironmentVariable("CLIENT_APP_URL") ?? "",
                        Environment.GetEnvironmentVariable("CLIENT_APP_URL_NO_PORT") ?? ""
                    )
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        return services;
    }

    /// <summary>
    /// Configures JWT bearer options for Keycloak authentication. This includes which token
    /// parameters are used for token claim validations, logging, and how to handle self-signed certificates.
    /// </summary>
    /// <param name="options">The JWT bearer options to configure.</param>
    /// <param name="isDevelopment">Flag indicating if the application is running in development mode.</param>
    private static void AddKeycloakJwtBearer(JwtBearerOptions options, bool isDevelopment)
    {
        var keycloakUrl = Environment.GetEnvironmentVariable("KEYCLOAK_URL") ?? "";
        
        options.RequireHttpsMetadata = false; // Set to false for self-signed certs
        options.MetadataAddress = $"{keycloakUrl}/realms/sprintpoker/.well-known/openid-configuration";
        options.RefreshOnIssuerKeyNotFound = true; // Refresh JWKS if the kid isn't found
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // "groups" is actually an array of Realm Roles from Keycloak. Create the Realm Roles
            // "sw_admin" and "sw_user", and then in the Keycloak client add the "groups" mapping
            // under Client scopes -> (client name)-dedicated -> Add mapper -> predefined -> groups
            RoleClaimType = "groups",
            NameClaimType = "preferred_username",
            // "account" is the default client ID from Keycloak that is used in the aud payload
            ValidAudience = "account",
            // https://stackoverflow.com/questions/60306175/bearer-error-invalid-token-error-description-the-issuer-is-invalid
            ValidateIssuer = !isDevelopment, // TODO: remove if unneeded after testing
            ClockSkew = TimeSpan.FromMinutes(5)
        };
        
        AddJwtCertsHandler(options, isDevelopment);
        AddJwtLoggingEvents(options);
    }

    /// <summary>
    /// Configures the certificate handler for JWT authentication, allowing self-signed certificates in development.
    /// </summary>
    /// <param name="options">The JWT bearer options to configure.</param>
    /// <param name="isDevelopment">Flag indicating if the application is running in development mode.</param>
    private static void AddJwtCertsHandler(JwtBearerOptions options, bool isDevelopment)
    {
        if (isDevelopment)
        {
            // Needed for self-signed certs
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        }
    }

    /// <summary>
    /// Adds JWT authentication event handlers for logging authentication failures and challenges.
    /// </summary>
    /// <param name="options">The JWT bearer options to configure logging for.</param>
    private static void AddJwtLoggingEvents(JwtBearerOptions options)
    {
        // Helpful logging
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"JWT auth failed: {ctx.Exception}");
                Console.WriteLine($"JWT MetadataAddress: {options.MetadataAddress}");
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine($"JWT challenge: {ctx.Error} - {ctx.ErrorDescription}");
                Console.WriteLine($"JWT MetadataAddress: {options.MetadataAddress}");
                return Task.CompletedTask;
            }
        };
    }
}