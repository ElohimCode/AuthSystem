using Common.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence.Context
{
    public class ApplicationDbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public ApplicationDbSeeder(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedDatabaseAsync()
        {
            await CheckAndApplyPendingMigrationAsync();
            await SeedRolesAsync();

            await SeedBasicUserAsync();

            await SeedAdminUserAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            string adminUserName = AppCredentials.Email[..AppCredentials.Email.IndexOf('@')].ToLowerInvariant();
            var adminUser = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = AppCredentials.Email,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = AppCredentials.Email.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true
            };

            if(!await _userManager.Users.AnyAsync(user => user.Email == AppCredentials.Email))
            {
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, AppCredentials.Password);
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, AppRoles.Basic)
                && !await _userManager.IsInRoleAsync(adminUser, AppRoles.Admin))
            {
                await _userManager.AddToRolesAsync(adminUser, AppRoles.DefaultRoles);
            }
        }

        private async Task SeedBasicUserAsync()
        {
            var basicUser = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johnd@abc.com",
                UserName = "johnd",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = "JOHND@ABC.COM",
                NormalizedUserName = "JOHND",
                IsActive = true
            };

            if (!await _userManager.Users.AnyAsync(u => u.Email == "johnd@abc.com"))
            {
                var password = new PasswordHasher<ApplicationUser>();
                basicUser.PasswordHash = password.HashPassword(basicUser, AppCredentials.Password);
                await _userManager.CreateAsync(basicUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(basicUser, AppRoles.Basic))
            {
                await _userManager.AddToRoleAsync(basicUser, AppRoles.Basic);
            }
        }

        private async Task SeedRolesAsync()
        {
            foreach(var roleName in AppRoles.DefaultRoles)
            {
                if (await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName) 
                    is not ApplicationRole role)
                {
                    role = new ApplicationRole
                    {
                        Name = roleName,
                        Description = $"{roleName} Role."
                    };
                    await _roleManager.CreateAsync(role);
                }
                if (roleName == AppRoles.Admin)
                {
                    await AssignPermissionsToRoleAsync(role, AppPermissions.AdminPermissions);
                }
                else if (roleName == AppRoles.Basic)
                {
                    await AssignPermissionsToRoleAsync(role, AppPermissions.BasicPermissions);
                }
            }
        
        }

        private async Task CheckAndApplyPendingMigrationAsync()
        {
            if(_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }
        }

        private async Task AssignPermissionsToRoleAsync(ApplicationRole role, IReadOnlyList<AppPermission> permissions)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(claim => claim.Type == AppClaim.Permission && claim.Value == permission.Name))
                {
                    await _context.RoleClaims.AddAsync(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = AppClaim.Permission,
                        ClaimValue = permission.Name,
                        Description = permission.Description,
                        Group = permission.Group
                    });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
