using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOM;
using ABS.Interfaces;

namespace REPO
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly IDataContext _context;

        public UsuarioRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Usuario usuario)
        {
            usuario.FechaCreacion = DateTime.Now;
            usuario.UsuarioCreacion = "Sistema"; // Esto debería venir del usuario logueado
            usuario.FechaModificacion = DateTime.Now;
            usuario.UsuarioModificador = "Sistema"; // Esto debería venir del usuario logueado

            // Simulación temporal
            usuario.ID = 1; // Simulamos un ID autogenerado (esto lo haría la DB real)
            return 1; // Simulamos que se insertó 1 fila
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            if (id == 1)
            {
                return new Usuario
                {
                    ID = id,
                    Nombre = "Nombre simulado",
                    Apellido = "Apellido",
                    Email = "email@exmaple.com",
                    Rol = new Rol { ID = 1, Detalle = "Admin" },
                    FechaCreacion = DateTime.Now.AddDays(-1),
                    UsuarioCreacion = "Sistema",
                    FechaModificacion = DateTime.Now,
                    UsuarioModificador = "Sistema"
                };
            }

            return null;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            // Ejemplo con Entity Framework:
            // return await _context.Set<Usuario>().ToListAsync();
            // Simulación temporal
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    ID = 1,
                    Nombre = "Nombre Simulado",
                    Apellido = "Apellido Simulado",
                    Email = "email@simulado.com",
                    Clave = "clave_simulada",
                    Estado = 0,
                    Rol = new Rol { ID = 1, Detalle = "Admin" },
                    FechaCreacion = DateTime.Now.AddDays(-1),
                    UsuarioCreacion = "Sistema",
                    FechaModificacion = DateTime.Now,
                    UsuarioModificador = "Sistema"
                }
                // Puedes agregar más usuarios simulados aquí si lo deseas
            };
            return usuarios;
        }

        // Implementación del método UpdateAsync
        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            // Ejemplo con Entity Framework:
            // _context.Set<Usuario>().Update(usuario);
            // await _context.SaveChangesAsync();
            // return true; // Si la operación tuvo éxito

            // Simulación temporal
            usuario.FechaModificacion = DateTime.Now;
            usuario.UsuarioModificador = "Sistema"; // Esto debería venir del usuario logueado
            // Simulamos que la actualización fue exitosa
            return true;
        }

        // Implementación del método DeleteAsync (Baja Lógica según el requerimiento)
        public async Task<bool> DeleteAsync(int id)
        {
            // Ejemplo con Entity Framework (baja lógica):
            // var usuario = await GetByIdAsync(id);
            // if (usuario != null)
            // {
            //     usuario.Estado = 1; // Marcar como inactivo
            //     usuario.FechaModificacion = DateTime.Now;
            //     usuario.UsuarioModificador = "Sistema"; // Usuario logueado
            //     await UpdateAsync(usuario); // Guardar los cambios
            //     return true;
            // }
            // return false; // Si no se encontró el usuario

            // Simulación temporal
            // Simulamos que se marcó como inactivo (Estado = 1)
            return true;
        }

        // Implementación del método ExistsAsync
        public async Task<bool> ExistsAsync(int id)
        {
            // Ejemplo con Entity Framework:
            // return await _context.Set<Usuario>().AnyAsync(u => u.ID == id);
            // Simulación temporal
            return id == 1; // Simulamos que solo existe el ID 1
        }

        // Implementación del método EmailExistsAsync
        public async Task<bool> EmailExistsAsync(string email)
        {
            // Ejemplo con Entity Framework:
            // return await _context.Set<Usuario>().AnyAsync(u => u.Email == email);
            // Simulación temporal
            return email == "email@simulado.com"; // Simulamos que este email ya existe
        }
    }
}
