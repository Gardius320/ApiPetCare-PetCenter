using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetCare.Application.Auth;
using PetCare.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PetCare.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email!),
                new Claim("nombre", user.FullName)
            };

            foreach (var rol in roles)
                claims.Add(new Claim("role", rol));

            var clave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var firmado = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: firmado
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public TimeSpan GetRefreshTokenDuration(IList<string> roles)
        {
            var sessionDurations = _configuration
                .GetSection("Jwt:SessionDurationDays")
                .Get<Dictionary<string, int>>() ?? new Dictionary<string, int>();

            int defaultDays = sessionDurations.TryGetValue("Default", out var dias) ? dias : 7;

            foreach (var role in roles)
            {
                if (sessionDurations.TryGetValue(role, out var diasRol))
                {
                    return TimeSpan.FromDays(diasRol);
                }
            }

            return TimeSpan.FromDays(defaultDays);
        }

        public string GenerateRefreshToken()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
