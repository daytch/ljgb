using ljgb.Common.Requests;
using ljgb.Common.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ljgb.API.Core
{
    public class Authentication
    {
        private readonly IConfiguration configuration;
        public Authentication(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public AuthenticationResponse BuildToken(UserRequest user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.user.Email),
                new Claim("CariMobilMurah", "LoJualGueBeli.com aja"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Kata_Paling_Rahasia"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Issuer"],
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return response = new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expired = DateTime.UtcNow.AddHours(1)
            };
        }


        public ClaimsPrincipal GetPrincipal(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Kata_Paling_Rahasia"]));

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception ex)
            {
                //appLogger.Error(ex);
                return null;
            }
        }

    }
}
