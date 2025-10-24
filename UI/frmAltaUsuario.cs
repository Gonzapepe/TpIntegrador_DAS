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

namespace UI
{
    public partial class frmAltaUsuario : Form
    {
        private readonly UsuarioServicio _servicio;
        public frmAltaUsuario()
        {
            InitializeComponent();

            string connectionString = "Server=.;Database=PeluqueriaDB;Trusted_Connection=true;";
            var repo = new DAL.UsuarioRepository(connectionString);
            _servicio = new UsuarioServicio(repo);

        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
                {
                    MessageBox.Show("Email inválido");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtClave.Text) || txtClave.Text.Length != 11)
                {
                    MessageBox.Show("La clave debe tener exactamente 11 caracteres");
                    return;
                }

                bool exito = await _servicio.AltaUsuarioAsync(txtNombre.Text,
                    txtApellido.Text,
                    txtEmail.Text,
                    txtClave.Text,
                    (int)cmbRol.SelectedValue);

                if (exito)
                {
                    MessageBox.Show("Usuario dado de alta correctamente");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el usuario");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
    }
}
