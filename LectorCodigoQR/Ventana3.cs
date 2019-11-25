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
    public partial class Ventana3 : Form
    {
        private SqlConnection conexiondb = new SqlConnection("Data Source =DESKTOP-2019; Initial Catalog = Almacen; Integrated Security = True");
        Conexion sql = new Conexion();
        public Ventana3()
        {
            InitializeComponent();
        }

        private void Ventana3_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            dataGridView1.DataSource = sql.MostrarDatos("Usuario");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            us.Select();
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ventana1 v = new Ventana1();
            v.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query2 = "INSERT INTO Usuario(Matricula,Email,Nombre,Apellido,UID,Nivel) values (@Matricula,@Email,@Nombre,@Apellido,@UID,@Nivel)";
            SqlCommand cmd2 = new SqlCommand(query2, conexiondb);
            conexiondb.Open();
            cmd2.Parameters.AddWithValue("@Matricula", matricula.Text);
            cmd2.Parameters.AddWithValue("@Email", correo.Text);
            cmd2.Parameters.AddWithValue("@Nombre", nombre.Text);
            cmd2.Parameters.AddWithValue("@Apellido", apellido.Text);
            cmd2.Parameters.AddWithValue("@UID", uid.Text);
            if(adm.Checked)
            {
                cmd2.Parameters.AddWithValue("@Nivel", "Admin");
            }
            else if(us.Checked)
            {
                cmd2.Parameters.AddWithValue("@Nivel", "User");
            }
            else
            {
                cmd2.Parameters.AddWithValue("@Nivel", "ERROR");
            }
            cmd2.ExecuteScalar();
            conexiondb.Close();
            dataGridView1.DataSource = sql.MostrarDatos("Usuario");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query2 = "UPDATE Usuario SET Email=@Email,Nombre=@Nombre,Apellido=@Apellido,UID=@UID,Nivel=@Nivel WHERE Matricula=@Matricula";
            SqlCommand cmd2 = new SqlCommand(query2, conexiondb);
            conexiondb.Open();
            cmd2.Parameters.AddWithValue("@Matricula", matricula.Text);
            cmd2.Parameters.AddWithValue("@Email", correo.Text);
            cmd2.Parameters.AddWithValue("@Nombre", nombre.Text);
            cmd2.Parameters.AddWithValue("@Apellido", apellido.Text);
            cmd2.Parameters.AddWithValue("@UID", uid.Text);
            if (adm.Checked)
            {
                cmd2.Parameters.AddWithValue("@Nivel", "Admin");
            }
            else if (us.Checked)
            {
                cmd2.Parameters.AddWithValue("@Nivel", "User");
            }
            else
            {
                cmd2.Parameters.AddWithValue("@Nivel", "ERROR");
            }
            cmd2.ExecuteScalar();
            conexiondb.Close();
            dataGridView1.DataSource = sql.MostrarDatos("Usuario");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            label3.Text = DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
