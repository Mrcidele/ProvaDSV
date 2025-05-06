
namespace Prova.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Local { get; set; } = null!;
        public DateTime Data { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } 
    }
}