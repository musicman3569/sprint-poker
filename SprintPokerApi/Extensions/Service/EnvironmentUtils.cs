namespace SprintPokerApi.Extensions.Service;

public static class EnvironmentUtils
{
    public static bool IsDevelopment(this IServiceCollection services)
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}