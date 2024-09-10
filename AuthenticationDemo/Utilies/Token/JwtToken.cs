using AuthenticationDemo.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationDemo.Utilies.Token
{
    public class JwtToken(IOptions<TokenModel> _options,IConfiguration _configuration): IJwtToken
    {
        public async Task<string> Generate(User _user)
        {
            var roleClaims = new List<Claim>();

            var roles = _user.UserRoles.Select(u => u.Role?.Name).ToList();

            foreach (var item in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, item));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name,$"{_user.FirstName} {_user.LastName}"),
                new Claim(ClaimTypes.Email,_user.Email),
            }
            .Union(roleClaims);

            var signingCredenial = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Key"]!)), SecurityAlgorithms.HmacSha256);

            var JWTSecuirtyToken = new JwtSecurityToken(
                    claims:claims,
                    signingCredentials:signingCredenial,
                    audience: _options.Value.Audience,
                    issuer: _options.Value.Issuer,
                    expires: DateTime.Now.AddDays(_options.Value.ExpireDuration));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(JWTSecuirtyToken));
        }
    }
}
