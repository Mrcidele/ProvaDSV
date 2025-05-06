using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Prova.Controllers
{
    [ApiController]
    [Route("eventos")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class EventosController : ControllerBase
    {
        private readonly AppDataContext _ctx;
        public EventosController(AppDataContext ctx) => _ctx = ctx;

        private int UsuarioId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpPost("criar")]
        public async Task<IActionResult> Criar([FromBody] Evento dto)
        {
            dto.UsuarioId = UsuarioId;
            _ctx.Eventos.Add(dto);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(null, new { id = dto.Id }, dto);
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var evs = await _ctx.Eventos
                .Where(e => e.UsuarioId == UsuarioId)
                .ToListAsync();
            return Ok(evs);
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Evento dto)
        {
            var eve = await _ctx.Eventos.FindAsync(id);
            if (eve == null || eve.UsuarioId != UsuarioId) return NotFound();

            eve.Nome = dto.Nome;
            eve.Local = dto.Local;
            eve.Data  = dto.Data;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var eve = await _ctx.Eventos.FindAsync(id);
            if (eve == null || eve.UsuarioId != UsuarioId) return NotFound();

            _ctx.Eventos.Remove(eve);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}