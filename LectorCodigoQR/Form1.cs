using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using BarcodeLib.BarcodeReader;
using AForge.Controls;
using AForge.Video;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Media;
using System.IO;

namespace LectorCodigoQR
{
    public partial class Form1 : Form
    {
        private int i = 0;



        public String previousItem = "";
        public String matricula = "";
        public Boolean allowControl = false;
        public String[] SKUS = { };
        public UdpClient Client;
        private SqlConnection conexiondb = new SqlConnection("Data Source =DESKTOP-2019; Initial Catalog = Almacen; Integrated Security = True");
        Conexion sql = new Conexion();
        public Form1(string ID)
        {
            matricula = ID;
            InitializeComponent();
        }
        public Form1()
        {


        }
        //VARIABLE PARA LISTA DE DISPOSITIVOS
        private FilterInfoCollection DISPOSITIVOS;
        //VARIABLE PARA FUENTE DE VIDEO
        private VideoCaptureDevice FUENTEDEVIDEO;
        private void Form1_Load(object sender, EventArgs e)
        {
            salida.Checked = true;
            SoundPlayer Player1 = new SoundPlayer();
            Player1.SoundLocation = "C:/Users/Alumno/Downloads/audioclip-1554823485-2827.wav";
            Player1.Play();

            timer4.Enabled = true;
            timer4.Start();


            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            textBox2.Text = matricula;
            timer3.Start();
            Client = new UdpClient(12345);
            Client.BeginReceive(DataReceived, null);
            timer2.Interval = 1000;
            timer2.Start();
            dgv.DataSource = sql.MostrarDatos("Articulo");
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //LISTAR DISPOSITIVOS DE ENTRADA DE VIDEO
            DISPOSITIVOS = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //CARGAR TODOS LOS DISPOSITIVOS AL COMBO
            foreach (FilterInfo X in DISPOSITIVOS)
            {
                comboBox1.Items.Add(X.Name);
            }
            comboBox1.SelectedIndex = 0;
            FUENTEDEVIDEO = new VideoCaptureDevice(DISPOSITIVOS[comboBox1.SelectedIndex].MonikerString);
            timer1.Enabled = true;
            //ESTABLECER EL DISPOSITIVO SELECCIONADO COMO FUENTE DE VIDEO
            FUENTEDEVIDEO = new VideoCaptureDevice(DISPOSITIVOS[comboBox1.SelectedIndex].MonikerString);
            //INICIALIZAR EL CONTROL
            videoSourcePlayer1.VideoSource = FUENTEDEVIDEO;
            //INICIAR RECEPCION DE IMAGENES
            videoSourcePlayer1.Start();
            textBox1.Enabled = true;
        }
        // CONTROL DE CAMARA DE VIDEO MEDIANTE TIMER1
        private void timer1_Tick(object sender, EventArgs e)
        {
            //ESTAR SEGUROS QUE HAY UNA IMAGEN DESDE LA WEBCAM
            if (videoSourcePlayer1.GetCurrentVideoFrame() != null)
            {
                //IBTENER IMAGEN DE LA WEBCAM
                Bitmap IMG = new Bitmap(videoSourcePlayer1.GetCurrentVideoFrame());
                //UTILIZAR LA LIBRERIA Y LEER EL CÓDIGO
                string[] RESULTADOS = BarcodeReader.read(IMG, BarcodeReader.QRCODE);
                //QUITAR LA IMAGEN DE MEMORIA
                IMG.Dispose();
                //OBTENER LAS LECTURAS CUANDO SE LEA ALGO
                if (RESULTADOS != null && RESULTADOS.Count() > 0 && RESULTADOS[0].Length > 0)
                {
                    if (!String.IsNullOrEmpty(RESULTADOS[0]))
                    {
                        //AGREGAR EL TEXTO OBTENIDO A LA LISTA
                        //Si el item scaneado anteriormente no es igual al que se esta escaneando actualmente (para evitar multiples envios)
                        string firstChar = RESULTADOS[0].Substring(0, 1);
                        //Solucion para arreglar el bug que inserta un 2 al primer caracter
                        if (System.Text.RegularExpressions.Regex.IsMatch(firstChar, "[^0-9]") && (previousItem != RESULTADOS[0]) | allowControl)
                        {
                            listBox1.Items.Add(RESULTADOS[0]);
                            previousItem = RESULTADOS[0];
                            allowControl = false;
                        }
                    }
                }
            }
        }

        // CONTROL DE ENTRADA DE CODIGO DE BARRAS MEDIANTE TEXTBOX1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 12)
            {
                textBox1.Enabled = false;
                // conexion a sql donde se comprueba el codigo de barra
                //PLACEHOLDER PARA DEMONSTRACION
                if (textBox1.Text.Length == 12)
                {
                    allowControl = true;
                    string query = "SELECT Nombre FROM Articulo WHERE Barras like '" + textBox1.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, conexiondb);
                    conexiondb.Open();
                    object returned = cmd.ExecuteScalar();
                    conexiondb.Close();
                    string itemName = "";
                    if (returned != null)
                    {
                        itemName = returned.ToString();
                        listBox1.Items.Add(itemName);
                        textBox1.Text = "";
                        textBox1.Enabled = true;
                        textBox1.Focus();
                        
                    }
                    else
                    {
                        textBox1.Text = "";
                        textBox1.Enabled = true;
                        textBox1.Focus();
                        MessageBox.Show("Item no encontrado!");
                    }

                }
                //PLACEHOLDER PARA DEMONSTRACION

            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }
        //MANEJO DE ENTRADA MEDIANTE NETWORKING
        private void DataReceived(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 12345);
            byte[] data;
            try
            {
                data = Client.EndReceive(ar, ref ip);

                if (data.Length == 0)
                    return; // No more to receive
                Client.BeginReceive(DataReceived, null);
            }
            catch (ObjectDisposedException)
            {
                return; // Connection closed
            }

            // Send the data to the UI thread
            this.BeginInvoke((Action<IPEndPoint, string>)DataReceivedUI, ip, Encoding.UTF8.GetString(data));
        }
        //MANEJO DE INTERFAZ DE DATOS RECIBIDOS
        private void DataReceivedUI(IPEndPoint endPoint, string data)
        {
            
            //textBox2.AppendText("[" + endPoint.ToString() + "] " + data + Environment.NewLine);
            textBox2.Text = "";
            textBox2.AppendText(data);
            string query = "SELECT Matricula FROM Usuario WHERE UID like '" + textBox2.Text + "'";
            SqlCommand cmd = new SqlCommand(query, conexiondb);
            conexiondb.Open();
            object returned = cmd.ExecuteScalar();
            conexiondb.Close();
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress serverAddr = IPAddress.Parse("169.254.142.185");
            IPEndPoint endP = new IPEndPoint(serverAddr, 11000);
            if (returned != null)
            {
                FUENTEDEVIDEO.SignalToStop();
                FUENTEDEVIDEO.WaitForStop();
                Client.Close();
                string text = "Authorized";
                byte[] send_buffer = Encoding.UTF8.GetBytes(text);
                sock.SendTo(send_buffer, endP);
                sock.Close();
                textBox2.Text = returned.ToString();
                ExecuteQuery();
                StartMenu s = new StartMenu();
                s.Show();
                this.Close();
            }
            else
            {
                Client.Close();
                string text = "Denied";
                byte[] send_buffer = Encoding.UTF8.GetBytes(text);
                sock.SendTo(send_buffer, endP);
                sock.Close();
                Client = new UdpClient(12345);
                Client.BeginReceive(DataReceived, null);
                textBox2.Text = matricula;
                //Mostrar algo en pantalla (Accesso denegado!) o algo asi.
            }
        }
        //CERRAR CONEXION????
        /*private void Stop_Click(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 12345);
            byte[] data;
            data = Client.EndReceive(ar, ref ip);
            Client.Close();
        }*/

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 12)
            {

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StartMenu st = new StartMenu();
            st.Show();
        }

        public void ExecuteQuery()
        {
            string query4 = "SELECT max(IDMovArt) FROM MovArt";
            SqlCommand cmd4 = new SqlCommand(query4, conexiondb);
            conexiondb.Open();
            object ret = cmd4.ExecuteScalar();
            conexiondb.Close();
            int nextID = (int)ret + 1;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string query = "SELECT SKU FROM Articulo WHERE Nombre like '" + listBox1.Items[i] + "'";
                SqlCommand cmd = new SqlCommand(query, conexiondb);
                conexiondb.Open();
                object returned = cmd.ExecuteScalar();
                conexiondb.Close();
                string query2 = "INSERT INTO MovArt(IDMovArt,Matricula,SKU,Cantidad) values (@IDMovArt,@Matricula,@SKU,@Cantidad)";
                SqlCommand cmd2 = new SqlCommand(query2, conexiondb);
                conexiondb.Open();
                cmd2.Parameters.AddWithValue("@IDMovArt", nextID);
                cmd2.Parameters.AddWithValue("@Matricula", textBox2.Text);
                cmd2.Parameters.AddWithValue("@SKU", (int)returned);
                cmd2.Parameters.AddWithValue("@Cantidad", "1");
                cmd2.ExecuteScalar();
                if(entrada.Checked)
                {
                    string query5 = "UPDATE Articulo SET Stock=Stock+1 WHERE Nombre like '" + listBox1.Items[i] + "'";
                    SqlCommand cmd5 = new SqlCommand(query5, conexiondb);
                    cmd5.ExecuteScalar();
                }
                else if(salida.Checked)
                {
                    string query5 = "UPDATE Articulo SET Stock=Stock-1 WHERE Nombre like '" + listBox1.Items[i] + "'";
                    SqlCommand cmd5 = new SqlCommand(query5, conexiondb);
                    cmd5.ExecuteScalar();
                }
                conexiondb.Close();
            }
            string query3 = "INSERT INTO Movimientos(Tipo,IDMovArt) values (@Tipo,@IDMovArt)";
            SqlCommand cmd3 = new SqlCommand(query3, conexiondb);
            conexiondb.Open();
            cmd3.Parameters.AddWithValue("@Tipo", "E");
            cmd3.Parameters.AddWithValue("@IDMovArt", nextID);
            cmd3.ExecuteScalar();
            conexiondb.Close();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            label3.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void timer4_Tick(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void timer5_Tick_1(object sender, EventArgs e)
        {
            

        }
    }
}