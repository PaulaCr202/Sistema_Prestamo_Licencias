using Identity.App.Data.Entities;
using Identity.App.DTOs.Auth;
using Identity.App.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.App.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
        {
            AppUser? user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                return null;
            }

            Microsoft.AspNetCore.Identity.SignInResult result =
                await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return null;
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IConfigurationSection jwt = _configuration.GetSection("JWT");
            string signKey = jwt["SignKey"] ?? throw new InvalidOperationException("Falta la configuración JWT:SignKey.");

            DateTime expiresAt = DateTime.UtcNow.AddMinutes(jwt.GetValue<int>("ExpirationMinutes", 60));
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(signKey));
            SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha256);

            string email = user.Email ?? string.Empty;

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("document", user.Document),
            ];

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            JwtSecurityToken token = new(issuer: jwt["Issuer"],
                                         audience: jwt["Audience"],
                                         claims: claims,
                                         expires: expiresAt,
                                         signingCredentials: credentials);

            return new AuthResponseDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresAtUTC = expiresAt
            };
        }
    }
}
