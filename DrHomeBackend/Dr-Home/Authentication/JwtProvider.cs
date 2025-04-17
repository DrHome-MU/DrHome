
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dr_Home.Authentication
{
    public class JwtProvider(IOptions<jwtOptions> options) : IJwtProvider
    {
        private readonly jwtOptions _jwtOptions = options.Value;

        public string GenerateToken(User user)
        {
            var authClaims = new Claim[]
         {
               new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
               new Claim(ClaimTypes.Name , user.FullName) ,
               new Claim(ClaimTypes.Role,user.role),
               new Claim(ClaimTypes.Email,user.Email)
         };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));


            var token = new JwtSecurityToken
            (
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddYears(1),
                signingCredentials: new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
