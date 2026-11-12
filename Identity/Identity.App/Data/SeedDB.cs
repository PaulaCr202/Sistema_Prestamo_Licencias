using Identity.App.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.App.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        private static readonly List<string> RoleNames = ["User", "Admin", "Catalog Manager"];

        private static readonly List<SeedUser> SeedUsers =
        [
            new SeedUser("admin@licencias.local", "1234*", "Admin", "User", "1111", ["Admin", "Catalog Manager"]),
            new SeedUser("catalogmanager@licencias.local", "1234", "Catalog", "Manager", "2222", ["Catalog Manager"]),
            new SeedUser("user@licencias.local", "1234", "Usuario", "Prueba", "3333", ["User"])
        ];

        public SeedDb(DataContext context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            foreach (string roleName in RoleNames)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    continue;
                }

                IdentityResult result = await _roleManager.CreateAsync(new AppRole(roleName));
                EnsureSuccess(result, $"Error creando el rol {roleName}");
            }

            foreach (SeedUser seedUser in SeedUsers)
            {
                await SeedUserAsync(seedUser);
            }
        }

        private async Task SeedUserAsync(SeedUser seedUser)
        {
            AppUser? user = await _userManager.FindByEmailAsync(seedUser.Email);
            if (user is not null)
            {
                return;
            }

            user = new AppUser
            {
                Email = seedUser.Email,
                UserName = seedUser.Email,
                FirstName = seedUser.FirstName,
                LastName = seedUser.LastName,
                Document = seedUser.Document,
                EmailConfirmed = true,
            };

            IdentityResult result = await _userManager.CreateAsync(user, seedUser.Password);
            EnsureSuccess(result, $"Error creando el usuario {seedUser.Email}");

            result = await _userManager.AddToRolesAsync(user, seedUser.RoleNames);
            EnsureSuccess(result, $"Error asignando roles al usuario {seedUser.Email}");
        }

        private static void EnsureSuccess(IdentityResult result, string message)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"{message}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        private record SeedUser(string Email,
                                string Password,
                                string FirstName,
                                string LastName,
                                string Document,
                                List<string> RoleNames);
    }
}
