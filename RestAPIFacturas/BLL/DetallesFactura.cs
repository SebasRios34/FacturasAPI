using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using DAL;
using Newtonsoft.Json;

namespace BLL
{
    public class DetallesFactura
    {
        #region Propiedades/Variables
        private int codigoFactura, monto, cantidad;
        private string nombreFactura;

        public int CodigoFactura
        {
            get { return codigoFactura; }
            set { codigoFactura = value; }
        }
        public int Monto
        {
            get { return monto; }
            set { monto = value; }
        }
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        public string NombreFactura
        {
            get { return nombreFactura; }
            set { nombreFactura = value; }
        }
        #endregion

        #region Propiedades/Variables de Conexion
        SqlConnection conn;
        string mensajeError;
        int numError;
        string sql;
        DataSet ds;
        #endregion

        public bool insertarDetallesFactura(string accion)
        {
            conn = DAL.DAL.traerConexion("public", ref mensajeError, ref numError);
            if (conn == null)
            {
                return false;
            }
            else
            {
                if (accion.Equals("Insertar"))
                {
                    sql = "insertarDetallesFactura";
                }
                else
                {
                    sql = "modificarDetallesFactura";
                }
                Parametros[] parametros = new Parametros[4];
                DAL.DAL.agregarEstructuraParametros(ref parametros, 0, "@CodigoFactura", SqlDbType.Int, codigoFactura);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 1, "@NombreFactura", SqlDbType.VarChar, nombreFactura);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 2, "@Monto", SqlDbType.Int, monto);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 3, "@Cantidad", SqlDbType.Int, cantidad);


                DAL.DAL.conectar(conn, ref mensajeError, ref numError);
                DAL.DAL.ejecutarSqlCommandParametros(conn, sql, true, parametros, ref mensajeError, ref numError);

                if (numError != 0)
                {
                    Console.WriteLine("NUMERO DE ERROR: " + numError.ToString() + "MENSAJE DE ERROR: " + mensajeError);
                    //HttpContext.Current.Response.Redirect("NUMERO DE ERROR: " + numError.ToString() + "MENSAJE DE ERROR: " + mensajeError);
                    DAL.DAL.desconectar(conn, ref mensajeError, ref numError);
                    return false;
                }
                else
                {
                    DAL.DAL.desconectar(conn, ref mensajeError, ref numError);
                    return true;
                }
            }
        }
    }
}
