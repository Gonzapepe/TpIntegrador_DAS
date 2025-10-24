using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOM;

namespace ABS.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<int> AddAsync(Usuario usuario);

        Task<Usuario?> GetByIdAsync(int id);

        Task<IEnumerable<Usuario>> GetAllAsync();

        Task<bool> UpdateAsync(Usuario usuario);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> EmailExistsAsync(string email);
    }
}
