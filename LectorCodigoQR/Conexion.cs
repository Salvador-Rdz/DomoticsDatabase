using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace LectorCodigoQR
{
    class Conexion
    {
        private SqlConnection conexiondb = new SqlConnection("Data Source =DESKTOP-2019; Initial Catalog = Almacen; Integrated Security = True");
        private DataSet ds;// guarda varias tablas y muestra datos

        public DataTable MostrarDatos(string table)
        {
            conexiondb.Open(); //Para hacer commands
            SqlCommand cmd = new SqlCommand("select * from " + table, conexiondb);
            SqlDataAdapter ad = new SqlDataAdapter(cmd); // Por si el comando es de tipo select
            ds = new DataSet(); //Borra todas las tablas
            ad.Fill(ds, "tabla");
            conexiondb.Close(); //Cerrar la conexion
            return ds.Tables["tabla"];//regresa la tabla
        }
        public DataTable Buscar(string ID)
        {
            conexiondb.Open(); //Para hacer commands
            SqlCommand cmd = new SqlCommand(string.Format("select * from Usuarios where IDUsuario like '%{0}%'", ID), conexiondb);
            SqlDataAdapter ad = new SqlDataAdapter(cmd); // Por si el comando es de tipo select
            ds = new DataSet(); //Borra todas las tablas
            ad.Fill(ds, "tabla");
            conexiondb.Close(); //Cerrar la conexion
            return ds.Tables["tabla"];//regresa la tabla
        }


        public bool Insertar(string query, SqlCommand cmd)
        {
            conexiondb.Open();
            int filasafec = cmd.ExecuteNonQuery();//Ver cuantas filas se afectaron
            conexiondb.Close();
            if (filasafec > 0)
            {
                return true;
            }
            else { return false; }
        }
        public bool Eliminar(string IDValue, string fieldName, string table)
        {
            string query = "delete from " + table + " where " + fieldName + " = @ID";
            SqlCommand cmd = new SqlCommand(query, conexiondb);
            cmd.Parameters.AddWithValue("@ID", IDValue);
            conexiondb.Open();
            int filasafec = cmd.ExecuteNonQuery();//Ver cuantas filas se afectaron
            conexiondb.Close();
            if (filasafec > 0) return true;
            else return false;
        }
    }
}