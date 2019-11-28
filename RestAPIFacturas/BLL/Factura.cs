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
    public class Factura
    {
        #region Propiedades/Variables 
        private int codigoFactura, codigoCliente;
        private DateTime fecha;

        public int CodigoFactura
        {
            get { return codigoFactura; }
            set { codigoFactura = value; }
        }

        public int CodigoCliente
        {
            get { return codigoCliente; }
            set { codigoCliente = value; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        #endregion

        #region Propiedades/Variables de Conexion
        SqlConnection conn;
        string mensajeError;
        int numError;
        string sql;
        DataSet ds;
        #endregion

        public string mostrarFactura()
        {
            conn = DAL.DAL.traerConexion("public", ref mensajeError, ref numError);
            if (conn == null)
            {
                return null;
            }
            else
            {
                sql = "mostrarFactura";
                ds = DAL.DAL.ejecutarDataSet(conn, sql, true, ref mensajeError, ref numError);
                if (numError != 0)
                {
                    return null;
                }
                else
                {
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
        }

        public string mostrarCodigoFactura()
        {
            conn = DAL.DAL.traerConexion("public", ref mensajeError, ref numError);
            if (conn == null)
            {
                return null;
            }
            else
            {
                sql = "mostrarCodigoFactura";
                ds = DAL.DAL.ejecutarDataSet(conn, sql, true, ref mensajeError, ref numError);
                if (numError != 0)
                {
                    return null;
                }
                else
                {
                    return JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
        }

        public bool insertarFactura(string accion)
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
                    sql = "insertarFactura";
                }
                else
                {
                    sql = "modificarFactura";
                }
                Parametros[] parametros = new Parametros[3];
                DAL.DAL.agregarEstructuraParametros(ref parametros, 0, "@CodigoFactura", SqlDbType.Int, codigoCliente);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 1, "@CodigoCliente", SqlDbType.Int, codigoFactura);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 2, "@Fecha", SqlDbType.Date, fecha);


                DAL.DAL.conectar(conn, ref mensajeError, ref numError);
                DAL.DAL.ejecutarSqlCommandParametros(conn, sql, true, parametros, ref mensajeError, ref numError);

                if (numError != 0)
                {
                    HttpContext.Current.Response.Redirect("NUMERO DE ERROR: " + numError.ToString() + "MENSAJE DE ERROR: " + mensajeError);
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
