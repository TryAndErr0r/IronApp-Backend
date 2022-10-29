using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using IronApp.Model.QuizEntityModel;

using Microsoft.IdentityModel.Logging;
using System.Linq;

namespace IronApp.UserManagement
{
    public class Auth : IJwtAuth
    {
        private readonly string key;
        public Auth(string key)
        {
            this.key = key;
        }
        public UserIdent ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidateIssuer = false;
            validationParameters.ValidateAudience = false;
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);


            var gameid = principal.Claims.FirstOrDefault(x => x.Type == "gameId").Value;
            var playerId = int.Parse(principal.Claims.FirstOrDefault(x => x.Type == "playerId").Value);
            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            return new UserIdent() {GameId = Guid.Parse( gameid),PlayerId = playerId,Email = email };
        }

        public UserIdent PrincipalToIdent(ClaimsIdentity identity)
        {
            var gameid = identity.Claims.FirstOrDefault(x => x.Type == "gameId").Value;
            var playerId = int.Parse(identity.Claims.FirstOrDefault(x => x.Type == "playerId").Value);
            var email = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            return new UserIdent() { GameId = Guid.Parse(gameid), PlayerId = playerId, Email = email };
        }

        public string Authentication(Guid gameid, Player player)
        {
            
            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim("gameId", gameid.ToString()),
                        new Claim("playerId", player.Id.ToString()),
                        new Claim(ClaimTypes.Email, player.Name),
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}
