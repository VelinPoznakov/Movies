using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Movie.Infrastructure;

public static class JWTConfiguration
{
    public static void AddShemaConfiguration(this AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = 
        options.DefaultChallengeScheme =
        options.DefaultForbidScheme =
        options.DefaultScheme = 
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    public static void AddJWTBearerOptions(this JwtBearerOptions options, IConfigurationManager configurationManager)
    {
        IConfigurationSection settingsSection = configurationManager.GetSection("JWT");

        string signingKey = settingsSection["SigningKey"] ?? string.Empty;
        
        if (System.Text.Encoding.UTF8.GetByteCount(signingKey) < 32)
            throw new InvalidOperationException("JWT:SigningKey is missing or shorter than 32 bytes");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = settingsSection.GetValue<bool>("ValidateIssuer"),
            ValidIssuer = settingsSection.GetValue<string>("Issuer"),
            ValidateAudience = settingsSection
                .GetValue<bool>("ValidateAudience"),
            ValidAudience = settingsSection.GetValue<string>("Audience"),
            ValidateLifetime = settingsSection
                .GetValue<bool>("ValidateLifetime"),
            ClockSkew = settingsSection.
                GetValue<TimeSpan>("ClockSkew"),
            ValidateIssuerSigningKey = settingsSection
                .GetValue<bool>("ValidateIssuerSigningKey"),
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(signingKey))
        };
    }
}