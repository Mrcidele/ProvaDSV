using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.Models;

namespace Prova.Controllers

{
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDataContext _ctx;
        private readonly IEventoRepository _EventoRepository;
        public UsuariosController(AppDataContext ctx, IEventoRepository EventoRepository)
        {
            _ctx = ctx;
            _EventoRepository = EventoRepository;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] Usuario dto)
        {
            if (await _ctx.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email já cadastrado.");

            dto.Senha = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(dto.Senha))
            );

            _ctx.Usuarios.Add(dto);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(null, new { id = dto.Id }, dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario dto)
        {
            var hash = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(dto.Senha))
            );

            var user = await _ctx.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Senha == hash);

            if (user == null) return Unauthorized("Credenciais inválidas.");

            var token = _EventoRepository.GerarToken(user);
            return Ok(new { token });
        }

        [HttpGet("listar")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Listar()
        {
            var users = await _ctx.Usuarios
                                  .Select(u => new { u.Id, u.Email, u.CriadoEm })
                                  .ToListAsync();
            return Ok(users);
        }
    }
}