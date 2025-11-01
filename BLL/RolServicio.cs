using ABS.Interfaces;
using DOM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class RolServicio
    {
        private readonly IRolRepository _repo;

        public RolServicio(IRolRepository repo)
        { 
            _repo = repo;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
