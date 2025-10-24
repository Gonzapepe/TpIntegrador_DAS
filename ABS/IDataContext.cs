using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOM;

namespace ABS.Interfaces
{
    public interface IDataContext
    {
        IUsuarioRepository UsuarioRepository { get; }

        Task<int> SaveChangeAsync();
    }
}
