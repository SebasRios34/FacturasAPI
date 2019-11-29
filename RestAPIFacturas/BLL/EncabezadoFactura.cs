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
    public class EncabezadoFactura
    {
        #region Propiedades/Variables
        private int codigoFactura;
        private string direccion, codigoPostal;

        public int CodigoFactura
        {
            get { return codigoFactura; }
            set { codigoFactura = value; }
        }
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { codigoPostal = value; }
        }
        #endregion

        #region Propiedades/Variables de Conexion
        SqlConnection conn;
        string mensajeError;
        int numError;
        string sql;
        DataSet ds;
        #endregion

        public bool insertarEncabezadoFactura(string accion)
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
                    sql = "insertarEncabezadoFactura";
                }
                else
                {
                    sql = "modificarEncabezadoFactura";
                }
                Parametros[] parametros = new Parametros[3];
                DAL.DAL.agregarEstructuraParametros(ref parametros, 0, "@CodigoFactura", SqlDbType.Int, codigoFactura);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 1, "@Direccion", SqlDbType.VarChar, direccion);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 2, "@CodigoPostal", SqlDbType.VarChar, codigoPostal);



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
