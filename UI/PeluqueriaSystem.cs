using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class PeluqueriaSystem : Form
    {
        public PeluqueriaSystem()
        {
            InitializeComponent();
        }

        private void PeluqueriaSystem_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void listaDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form.GetType() == typeof(frmUsuarios))
                {
                    form.Activate();
                    return;
                }
            }

            // Si no está abierto, creamos uno nuevo
            frmUsuarios f = new frmUsuarios(); // Inyectar dependencias aquí si usas IoC
            f.MdiParent = this;
            f.Show();
            // Centrar el formulario hijo dentro del MDI (opcional)
            f.Left = (this.ClientSize.Width - f.Width) / 2;
            f.Top = (this.ClientSize.Height - f.Height) / 2;
        }
    }
}
