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
    public partial class Ventana2 : Form
    {
        private SqlConnection conexiondb = new SqlConnection("Data Source =DESKTOP-2019; Initial Catalog = Almacen; Integrated Security = True");
        Conexion sql = new Conexion();
        public Ventana2()
        {
            InitializeComponent();
        }

        private void Ventana2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            dataGridView1.DataSource = sql.MostrarDatos("Articulo");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            timer1.Start();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ventana1 v = new Ventana1();
            v.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            label3.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query2 = "INSERT INTO Articulo(SKU,Medida,Descripción,Nombre,FechaDeAlta,QR,Barras,Stock) values (@SKU,@Medida,@Descripción,@Nombre,@FechaDeAlta,@QR,@Barras,@Stock)";
            SqlCommand cmd2 = new SqlCommand(query2, conexiondb);
            conexiondb.Open();
            cmd2.Parameters.AddWithValue("@SKU", sku.Text);
            cmd2.Parameters.AddWithValue("@Medida", medida.Text);
            cmd2.Parameters.AddWithValue("@Descripción", descripcion.Text);
            cmd2.Parameters.AddWithValue("@Nombre", nombre.Text);
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd");
            cmd2.Parameters.AddWithValue("@FechaDeAlta", sqlFormattedDate);
            cmd2.Parameters.AddWithValue("@QR", qr.Text);
            cmd2.Parameters.AddWithValue("@Barras", barras.Text);
            cmd2.Parameters.AddWithValue("@Stock", stock.Text);
            cmd2.ExecuteScalar();
            conexiondb.Close();
            dataGridView1.DataSource = sql.MostrarDatos("Articulo");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query2 = "UPDATE Articulo SET Medida=@Medida,Descripción=@Descripción,Nombre=@Nombre,QR=@QR,Barras=@Barras,Stock=@Stock WHERE SKU=@SKU";
            SqlCommand cmd2 = new SqlCommand(query2, conexiondb);
            conexiondb.Open();
            cmd2.Parameters.AddWithValue("@SKU", sku.Text);
            cmd2.Parameters.AddWithValue("@Medida", medida.Text);
            cmd2.Parameters.AddWithValue("@Descripción", descripcion.Text);
            cmd2.Parameters.AddWithValue("@Nombre", nombre.Text);
            cmd2.Parameters.AddWithValue("@QR", qr.Text);
            cmd2.Parameters.AddWithValue("@Barras",barras.Text);
            cmd2.Parameters.AddWithValue("@Stock", stock.Text);
            cmd2.ExecuteScalar();
            conexiondb.Close();
            dataGridView1.DataSource = sql.MostrarDatos("Articulo");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query2 = "DELETE from Articulo WHERE SKU=@SKU";
            SqlCommand cmd2 = new SqlCommand(query2, conexiondb);
            conexiondb.Open();
            cmd2.Parameters.AddWithValue("@SKU", sku2.Text);
            cmd2.ExecuteScalar();
            conexiondb.Close();
            dataGridView1.DataSource = sql.MostrarDatos("Articulo");
        }
    }
}
