using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DOM;

namespace UI
{
    public partial class frmAltaUsuario : Form
    {
        private readonly UsuarioServicio _servicio;
        private readonly bool _modoEdicion;
        private Usuario _usuarioEnEdicion;
        public frmAltaUsuario(UsuarioServicio servicio, bool modoEdicion, Usuario usuarioAEliminar)
        {
            InitializeComponent();

            string connectionString = "Server=.;Database=PeluqueriaDB;Trusted_Connection=true;";
            var repo = new DAL.UsuarioRepository(connectionString);
            _servicio = servicio;
            _modoEdicion = modoEdicion;
            _usuarioEnEdicion = usuarioAEliminar;

            if (_modoEdicion && _usuarioEnEdicion != null)
            {
                this.Text = "Modificar Usuario"; // Cambia el título del formulario
                CargarDatosUsuario(_usuarioEnEdicion); // Carga los datos del usuario en los controles
                btnEliminar.Visible = true; // Muestra el botón de eliminar (baja lógica)
            }
            else
            {
                this.Text = "Alta de Usuario"; // Cambia el título del formulario
                btnEliminar.Visible = false; // Oculta el botón de eliminar en modo alta
                // El ID se dejará en 0, que es correcto para alta, y txtID es ReadOnly
                chkEstado.Checked = true; // Por defecto, en alta, el estado es Activo (0)
            }
        }

        // Método para cargar los datos de un usuario existente en los controles del formulario
        private void CargarDatosUsuario(Usuario usuario)
        {
            txtID.Text = usuario.ID.ToString();
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtEmail.Text = usuario.Email;
            // NO cargamos la clave por seguridad
            chkEstado.Checked = (usuario.Estado == 0); // 0 = Activo (checkbox marcado), 1 = Inactivo (checkbox desmarcado)
            cmbRol.SelectedItem = usuario.Rol; // Asumiendo que los valores del combo coinciden con los roles en BD
        }

        // Evento del botón Guardar
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_modoEdicion)
                {
                    // Modificar un usuario existente
                    if (_usuarioEnEdicion == null) return; // Protección adicional

                    // Validaciones básicas (pueden mejorarse)
                    if (string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        MessageBox.Show("Los campos Apellido, Nombre y Email son obligatorios.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Actualizar los datos del objeto _usuarioEnEdicion con los valores del formulario
                    _usuarioEnEdicion.Nombre = txtNombre.Text;
                    _usuarioEnEdicion.Apellido = txtApellido.Text;
                    _usuarioEnEdicion.Email = txtEmail.Text;
                    // Opcional: Permitir cambiar la clave en modo edición
                    // _usuarioEnEdicion.Clave = EncriptacionServicio.EncriptarSHA256(nuevaClaveTextBox.Text);
                    _usuarioEnEdicion.Estado = chkEstado.Checked ? 0 : 1; // 0 = Activo, 1 = Inactivo
                    _usuarioEnEdicion.Rol = (int)cmbRol.SelectedItem;

                    // Llamar al servicio para actualizar en la base de datos
                    bool exito = await _servicio.ModificarUsuarioAsync(_usuarioEnEdicion);

                    if (exito)
                    {
                        MessageBox.Show("Usuario modificado correctamente.");
                        this.DialogResult = DialogResult.OK; // Indica éxito
                        this.Close(); // Cierra el formulario
                    }
                    else
                    {
                        MessageBox.Show("No se pudo modificar el usuario.");
                    }
                }
                else
                {
                    // Dar de alta un nuevo usuario (tu código actual adaptado a async)
                    // Validaciones básicas (pueden mejorarse)
                    if (string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
                    {
                        MessageBox.Show("Los campos Apellido, Nombre, Email y Clave son obligatorios.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Llamar al servicio para crear el nuevo usuario en la base de datos
                    bool exito = await _servicio.AltaUsuarioAsync(
                        txtNombre.Text,
                        txtApellido.Text,
                        txtEmail.Text,
                        txtClave.Text, // Asumiendo que txtClave es visible y obligatorio en modo alta
                        (int)cmbRol.SelectedItem
                    );

                    if (exito)
                    {
                        MessageBox.Show("Usuario dado de alta correctamente");
                        this.DialogResult = DialogResult.OK; // Indica éxito
                        this.Close(); // Cierra el formulario
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el usuario");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Evento del botón Cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close(); // Simplemente cierra el formulario
        }

        // Evento del botón Eliminar (Baja Lógica)
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_modoEdicion && _usuarioEnEdicion != null)
            {
                var confirmResult = MessageBox.Show($"¿Está seguro de que desea eliminar (baja lógica) al usuario {_usuarioEnEdicion.Nombre} {_usuarioEnEdicion.Apellido}?",
                                                     "Confirmar Eliminación",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Llamar al servicio para realizar la baja lógica
                        bool exito = await _servicio.EliminarUsuarioAsync(_usuarioEnEdicion.ID);

                        if (exito)
                        {
                            MessageBox.Show("Usuario eliminado (baja lógica) correctamente.");
                            this.DialogResult = DialogResult.OK; // Indica éxito
                            this.Close(); // Cierra el formulario
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el usuario.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void frmAltaUsuario_Load(object sender, EventArgs e)
        {
            // Cargar los valores posibles para el rol (0 a 9)
            for (int i = 0; i <= 9; i++)
            {
                cmbRol.Items.Add(i);
            }

            if (cmbRol.Items.Count > 0)
            {
                cmbRol.SelectedIndex = 0;
            }
        }
    }
}
