using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Movie.Infrastructure;

public static class CorsPolicies
{
    public const string CORSPOLICYNAME = "AllowReactApp";
    
    public static void AddCorsOptions(this CorsOptions options, string[] cors)
    {
        options.AddPolicy(CORSPOLICYNAME, policy =>
        {
            policy.WithOrigins(cors)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    }
}