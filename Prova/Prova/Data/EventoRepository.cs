using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Prova.Models;

namespace Prova.Data
{
    public class EventoRepository : IEventoRepository
    {
        private readonly IConfiguration _config;
        public EventoRepository(IConfiguration config) => _config = config;

        public string GerarToken(Usuario usuario)
        {
            var jwt  = _config.GetSection("Jwt");
            var key  = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt["Key"]!));
            var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["DurationMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}