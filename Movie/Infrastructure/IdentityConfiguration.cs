using Microsoft.AspNetCore.Identity;
using Movie.Data.Seed;

namespace Movie.Infrastructure;

public static class IdentityConfiguration
{
    public static void AddIdentityConfiguration(this IdentityOptions options, IConfigurationManager configurationManager)
    {
        IConfigurationSection settingsSection = configurationManager.GetSection("Identity");

        options.Password.RequireDigit = settingsSection
            .GetValue<bool>("Password:RequireDigit");
        
        options.Password.RequireLowercase = settingsSection
            .GetValue<bool>("Password:RequireLowercase");
        
        options.Password.RequireUppercase = settingsSection
            .GetValue<bool>("Password:RequireUppercase");
        
        options.Password.RequireNonAlphanumeric = settingsSection
            .GetValue<bool>("Password:RequireNonAlphanumeric");
        
        options.Password.RequiredLength = settingsSection
            .GetValue<int>("Password:RequiredLength");

        options.User.RequireUniqueEmail = settingsSection
            .GetValue<bool>("User:RequireUniqueEmail");

        options.Lockout.MaxFailedAccessAttempts = settingsSection
            .GetValue<int>("Lockout:MaxFailedAccessAttempts");
        
        options.Lockout.DefaultLockoutTimeSpan = settingsSection
            .GetValue<TimeSpan>("Lockout:DefaultLockoutTimeSpan");
        
        options.Lockout.AllowedForNewUsers = settingsSection.
            GetValue<bool>("Lockout:AllowedForNewUsers");
    }

    public static void RoleSeeder(this IApplicationBuilder applicationBuilder,
        IConfiguration configuration)
    {
        bool seedRoles = configuration.GetValue<bool>("SeedRoles");
        
        if(!seedRoles)
            return;
        
        using IServiceScope scope = applicationBuilder
            .ApplicationServices
            .CreateScope();

        IIdentitySeeder roleSeeder = scope
            .ServiceProvider
            .GetRequiredService<IIdentitySeeder>();

        roleSeeder.SeedRolesAsync()
            .GetAwaiter()
            .GetResult();
    }
}