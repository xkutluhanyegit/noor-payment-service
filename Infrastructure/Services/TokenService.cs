using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Persistence.Repositories.YildatRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IYildatRepository _yildatRepository;
        public TokenService(IOptions<JwtSettings> jwtSettings,IYildatRepository yildatRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _yildatRepository = yildatRepository;
        }

        public string GenerateAccessToken(string tckn, string phone)
        {
            var claims = new []{
                new Claim("tckn",tckn),
                new Claim("phone",phone),
                new Claim("purpose","payment"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpirationMinutes)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateSmsVerificationToken(string tckn)
        {
            var claims = new []{
                new Claim("tckn",tckn),
                new Claim("phone",_yildatRepository.GetByFilterAsync(p=>p.Tckn == tckn).Result.TelNo),
                new Claim("purpose","sms-verification"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpirationMinutes)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}