using TCC.Domain.Entities;

namespace TCC.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByCpfAsync(string cpf);
    }
}
