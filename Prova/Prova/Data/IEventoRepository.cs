using Prova.Models;

namespace Prova.Data
{
    public interface IEventoRepository
    {
        string GerarToken(Usuario usuario);
    }
}