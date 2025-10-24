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

        public async Task<bool> AltaUsuarioAsync(string nombre, string apellido, string email, string clave, int rol)
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
    }
}
