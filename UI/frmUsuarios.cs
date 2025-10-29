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
using DAL;
using ABS;
using DOM;
using ABS.Interfaces;

namespace UI
{
    public partial class frmUsuarios : Form
    {
        private readonly UsuarioServicio _servicio;
        public frmUsuarios()
        {
            InitializeComponent();

            string connectionString = "Server=.\\SQLEXPRESS;Database=PeluqueriaDB;Trusted_Connection=true;TrustServerCertificate=True;";
            IUsuarioRepository repo = new UsuarioRepository(connectionString);
            _servicio = new UsuarioServicio(repo);
        }

        private async void frmUsuarios_Load(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
        }

        private async Task CargarUsuariosAsync()
        {
            try
            {
                IEnumerable<Usuario> usuarios = await _servicio.ObtenerTodosAsync();

                dgvUsuarios.DataSource = usuarios.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaUsuario f = new frmAltaUsuario(_servicio, modoEdicion: false, usuarioAEliminar: null);
            f.MdiParent = this.MdiParent;
            f.FormClosed += (s, args) => CargarUsuariosAsync(); // Recargar lista al cerrar el formulario de alta
            f.Show();
            f.Left = (this.MdiParent.ClientSize.Width - f.Width) / 2;
            f.Top = (this.MdiParent.ClientSize.Height - f.Height) / 2;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var usuarioSeleccionado = (Usuario)dgvUsuarios.Rows[e.RowIndex].DataBoundItem;

                frmAltaUsuario f = new frmAltaUsuario(_servicio, modoEdicion: true, usuarioAEliminar: usuarioSeleccionado);
                f.MdiParent = this.MdiParent;
                f.FormClosed += (s, args) => CargarUsuariosAsync();
                f.Show();
                f.Left = (this.MdiParent.ClientSize.Width - f.Width) / 2;
                f.Top = (this.MdiParent.ClientSize.Height - f.Height) / 2;
            }
        }
    }
}
