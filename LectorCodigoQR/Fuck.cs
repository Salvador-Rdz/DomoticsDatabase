using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LectorCodigoQR
{
    public partial class Fuck : Form
    {
        public Fuck()
        {
            InitializeComponent();
        }

        private void Fuck_Load(object sender, EventArgs e)
        {
            StartMenu s = new StartMenu();
            s.Show();
            this.Opacity = 0.0f;

            this.ShowInTaskbar = false;
        }
    }
}
