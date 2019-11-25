using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace LectorCodigoQR
{
    public partial class Management : Form
    {
        private SqlConnection conexiondb = new SqlConnection("Data Source =DESKTOP-2019; Initial Catalog = Almacen; Integrated Security = True");
        Conexion sql = new Conexion();
        public Management()
        {
            InitializeComponent();
        }

        private void Management_Load(object sender, EventArgs e)
        {
            dgv.DataSource = sql.MostrarDatos("Articulo");
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Articulo SET values QR='@QR', Barras='@Barras' WHERE SKU ='@ID'";
        }
    }
}
