

namespace Prova.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    }
}