using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOM
{
    public class Usuario
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public int Estado { get; set; } // 0: activo, 1: inactivo
        public Rol Rol { get; set; } // valor numérico (0-9) - asignado por el sistema

        // Campos para auditoría (quién y cuándo se creó/modificó)
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificador { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }

        public Usuario()
        {

        }

        public Usuario(int id, string nombre, string apellido, string email, string clave, int estado, Rol rol, string usuarioCreacion, DateTime fechaCreacion, string usuarioModificador, DateTime fechaModificacion)
         {
             ID = id;
             Nombre = nombre;
             Apellido = apellido;
             Email = email;
             Clave = clave;
             Estado = estado;
             Rol = rol;
             UsuarioCreacion = usuarioCreacion;
             FechaCreacion = fechaCreacion;
             UsuarioModificador = usuarioModificador;
             FechaModificacion = fechaModificacion;
         }
}


}
