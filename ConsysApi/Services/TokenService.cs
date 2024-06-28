using ConsysApi.Data.Model;
using ConsysApi.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConsysApi.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;        
        }

        public string GenereteToken(Usuarios user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("JwtKey"));
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = new ClaimsIdentity(GetUserClaimDefault(user)),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private IEnumerable<Claim> GetUserClaimDefault(Usuarios user)
        {
            yield return new Claim("CRUD", user.Crud);
            yield return new Claim(ClaimTypes.Name, user.NomeId);
        }
    }
}
