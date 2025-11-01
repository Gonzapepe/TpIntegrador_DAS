using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOM;
using SERV;
using ABS.Interfaces;


namespace BLL
{
    public class UsuarioServicio
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioServicio(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> AltaUsuarioAsync(string nombre, string apellido, string email, string clave, Rol rol)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Email inválido");
            }

            if (string.IsNullOrWhiteSpace(clave) || clave.Length != 11)
                throw new ArgumentException("La clave debe tener exactamente 11 caracteres");

            // Verificar si el email ya existe
            if (await _repo.EmailExistsAsync(email))
                throw new InvalidOperationException("Ya existe un usuario con ese email");

            var usuario = new Usuario
            {
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Clave = EncriptacionServicio.EncriptarSHA256(clave),
                Estado = 0, // activo
                Rol = rol,
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = Environment.UserName
            };

            int id = await _repo.AddAsync(usuario);

            return id > 0;
        }


        public async Task<bool> ModificarUsuarioAsync(Usuario usuario)
        {
            // Validaciones básicas antes de actualizar
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (usuario.ID <= 0)
                throw new ArgumentException("El ID del usuario debe ser positivo.", nameof(usuario));

            if (string.IsNullOrWhiteSpace(usuario.Email) || !usuario.Email.Contains("@"))
                throw new ArgumentException("Email inválido.", nameof(usuario));

            // Opcional: Verificar si el email ya existe para otro usuario (distinto al que se edita)
            // bool emailOtroUsuario = await _repo.EmailExistsAsync(usuario.Email);
            // if (emailOtroUsuario && (await _repo.GetByIdAsync(usuario.ID))?.Email != usuario.Email)
            //     throw new InvalidOperationException("Ya existe otro usuario con ese email.");

            // Opcional: Encriptar la clave si se modificó
            // if (!string.IsNullOrEmpty(usuario.Clave) && usuario.Clave.Length == 11)
            // {
            //     usuario.Clave = EncriptacionServicio.EncriptarSHA256(usuario.Clave);
            // }
            // Si la clave no se modifica en la edición, no se toca el campo Clave en la BD

            // Llama directamente al repositorio para ejecutar la actualización
            bool exito = await _repo.UpdateAsync(usuario);
            return exito;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            // Opcional: Validar si el ID es positivo
            if (id <= 0)
                throw new ArgumentException("El ID del usuario debe ser positivo.", nameof(id));

            // Opcional: Verificar si el usuario existe antes de intentar eliminarlo
            // if (!await _repo.ExistsAsync(id)) throw new InvalidOperationException("Usuario no encontrado");

            // Llama directamente al repositorio para ejecutar la baja lógica
            bool exito = await _repo.DeleteAsync(id);
            return exito;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
