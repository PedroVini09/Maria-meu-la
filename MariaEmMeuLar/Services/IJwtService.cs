using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Services
{
    public interface IJwtService
    {
        string GerarToken(UsuarioAdmin usuario);
    }
}