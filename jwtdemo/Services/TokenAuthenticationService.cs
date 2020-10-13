using jwtdemo.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwtdemo.Services
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUserService userService, IOptionsSnapshot<TokenManagement> tokenManagement)
        {
            _userService = userService;
            _tokenManagement = tokenManagement.Value;
        }

        public AccessToken DecodeToken(string accesstoken)
        {
            AccessToken token = new AccessToken();
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;
            if (string.IsNullOrEmpty(_tokenManagement.Secret))
            {
                throw new Exception("秘钥为空");
            }

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Secret));

            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = _tokenManagement.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = _tokenManagement.Audience,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                principal = handler.ValidateToken(accesstoken, parameters, out validToken);
                var validJwt = validToken as JwtSecurityToken;
                if (validJwt == null)
                {
                    throw new ArgumentException("Invalid JWT");
                }

                if (!validJwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{SecurityAlgorithms.HmacSha256}'");
                }

                token.UserId = validJwt.Claims.FirstOrDefault(t => JwtClaimTypes.UserId == t.Type)?.Value ?? string.Empty;
                token.UserName = validJwt.Claims.FirstOrDefault(t => JwtClaimTypes.UserName == t.Type)?.Value ?? string.Empty;
                token.AccountName = validJwt.Claims.FirstOrDefault(t => JwtClaimTypes.AccountName == t.Type)?.Value ?? string.Empty;
                token.Company = validJwt.Claims.First(t => JwtClaimTypes.Company == t.Type)?.Value ?? string.Empty;

                token.Role = validJwt.Claims.FirstOrDefault(t => JwtClaimTypes.Role == t.Type)?.Value ?? string.Empty;
                token.Name = validJwt.Claims.FirstOrDefault(t => JwtClaimTypes.Role == t.Type)?.Value ?? string.Empty;

                return token;
            }
            catch (SecurityTokenValidationException ex)
            {
                return null;
            }
            catch (ArgumentException ex)
            {
                return null;
            }
        }

        public bool IsAuthenticated(LoginDto request, out string token)
        {
            token = string.Empty;
            if (!_userService.IsValid(request))
                return false;
            var claims = new[]
            {
                new Claim(JwtClaimTypes.UserName,request.Username),
                new Claim(JwtClaimTypes.Role,"admin"),
                new Claim(JwtClaimTypes.Company, "31"),
                new Claim(JwtClaimTypes.UserName, "管保洲"),
                new Claim(JwtClaimTypes.AccountName, "velenooo"),
                new Claim(JwtClaimTypes.UserId, "v18502150402")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return true;
        }
    }
}
