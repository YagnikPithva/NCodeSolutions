using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _cfg;

    public TokenService(IConfiguration cfg) { _cfg = cfg; }

    public (string Token, DateTime ExpiresAt) Create(User user)
    {
        var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role)
            };

        if (user.OrganizationId is int orgId) claims.Add(new Claim("orgId", orgId.ToString()));
        if (user.EmployeeId is int empId) claims.Add(new Claim("empId", empId.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpiryMinutes"]!));

        var token = new JwtSecurityToken(_cfg["Jwt:Issuer"], _cfg["Jwt:Audience"],
                                         claims, expires: expires, signingCredentials: creds);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

}
