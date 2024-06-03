using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObject.Entities;
using Microsoft.IdentityModel.Tokens;

namespace PetHealthCareSystemAPI.Helpers
{
    public interface IJwtGenerator
    {
        string CreateJwtToken(User account, int hours);
    }

    public class JwtHelper : IJwtGenerator
    {
        private readonly string _secretKey;

        public JwtHelper(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string CreateJwtToken(User account, int hours)
        {
            // generate token that is valid for n times
            var tokenHandler = new JwtSecurityTokenHandler();

            // define token
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            SecurityTokenDescriptor tokenDescriptor;

            tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "",
                Issuer = "",
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("Username", account.Username),
                        new Claim("UserId", account.Id.ToString()),
                        new Claim("Email", account.Email),
                        new Claim("Phone", account.Phone),
                        new Claim("Role", account.Role.ToString())
                }),
                // now + 1 hour
                Expires = DateTime.UtcNow.AddHours(hours),
                SigningCredentials = credential
            };

            // use token handler to create token from descriptor info
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
