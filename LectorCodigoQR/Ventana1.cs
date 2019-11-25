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
    public partial class Ventana1 : Form
    {
        private SqlConnection conexiondb = new SqlConnection("Data Source =DESKTOP-2019; Initial Catalog = Almacen; Integrated Security = True");
        Conexion sql = new Conexion();
        public Ventana1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Ventana1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            dataGridView1.DataSource = sql.MostrarDatos("MovArt");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ventana3 v = new Ventana3();
            v.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ventana2 v = new Ventana2();
            v.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartMenu s = new StartMenu();
            s.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            label3.Text = DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
