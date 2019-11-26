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
    public class Cliente
    {
        #region Propiedades/Variables
        private int codigoCliente, year;
        private string nombreEstudiante, nombreProfesor, nombreTaller;

        public int CodigoCliente
        {
            get { return codigoCliente; }
            set { codigoCliente = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public string NombreEstudiante
        {
            get { return nombreEstudiante; }
            set { nombreEstudiante = value; }
        }
        public string NombreProfesor
        {
            get { return nombreProfesor; }
            set { nombreProfesor = value; }
        }

        public string NombreTaller
        {
            get { return nombreTaller; }
            set { nombreTaller = value; }
        }
        #endregion

        #region Propiedas/Variables de Conexion
        SqlConnection conn;
        string mensajeError;
        int numError;
        string sql;
        DataSet ds;
        #endregion

        public string mostrarCliente()
        {
            conn = DAL.DAL.traerConexion("public", ref mensajeError, ref numError);
            if (conn == null)
            {
                return null;
            }
            else
            {
                sql = "mostrarCliente";
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

        public string mostrarCodigoCliente()
        {
            conn = DAL.DAL.traerConexion("public", ref mensajeError, ref numError);
            if (conn == null)
            {
                return null;
            }
            else
            {
                sql = "mostrarCodigoCliente";
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

        public bool insertarCliente(string accion)
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
                    sql = "insertarCliente";
                }
                else
                {
                    sql = "modificarCliente";
                }
                Parametros[] parametros = new Parametros[5];
                DAL.DAL.agregarEstructuraParametros(ref parametros, 0, "@CodigoCliente", SqlDbType.Int, codigoCliente);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 1, "@NombreEstudiante", SqlDbType.VarChar, nombreEstudiante);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 2, "@NombreProfesor", SqlDbType.VarChar, nombreProfesor);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 3, "@NombreTaller", SqlDbType.VarChar, nombreTaller);
                DAL.DAL.agregarEstructuraParametros(ref parametros, 4, "@Year", SqlDbType.Int, year);

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
