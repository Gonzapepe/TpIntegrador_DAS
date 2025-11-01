using DOM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABS.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetAllAsync();
    }
}
