using Microsoft.AspNetCore.Identity;

namespace Movie.Data.Seed
{
    public class IdentitySeeder: IIdentitySeeder
    {
        private readonly string[] ApplicationRoles =
        [
            "User",
            "Admin",
            "SuperAdmin"
        ];
        
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ILogger<IdentitySeeder> _logger;

        public IdentitySeeder(
            RoleManager<IdentityRole<Guid>> roleManager,
            ILogger<IdentitySeeder> logger)
        {
            this._roleManager = roleManager;
            this._logger = logger;
        }

        public async Task SeedRolesAsync()
        {
            foreach(string role in ApplicationRoles)
            {
                bool roleExist = await _roleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                    IdentityRole<Guid> newRole = new IdentityRole<Guid>(role);
                    IdentityResult identityRole = await _roleManager.CreateAsync(newRole);

                    if (!identityRole.Succeeded)
                    {
                        _logger.LogError($"There is a problem seeding role {role}: {identityRole.Errors}");
                        throw new Exception();
                    }
                        
                }
            }
        }
    }
}
