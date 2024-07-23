using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sharpist.Server.Domain.Entities;
using Sharpist.Server.Service.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sharpist.Server.Service.Services;

public class AuthService : IAuthService
{
    private readonly IConfigurationSection configuration;
    public AuthService(IConfiguration configuration)
    {
        this.configuration = configuration.GetSection("Jwt");
    }

    public string GenerateToken(HR user)
    {
        var claims = new[]
        {

            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name+ " " + user.LastName),
            new Claim("Email", user.Email)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(configuration["Issuer"], configuration["Audience"], claims,
            expires: DateTime.Now.AddMinutes(double.Parse(configuration["Lifetime"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
