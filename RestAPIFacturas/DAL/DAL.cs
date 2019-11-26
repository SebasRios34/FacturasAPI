using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DAL
{
    #region Estructura de Parametros
    public struct Parametros
    {
        public string nombreParametro;
        public SqlDbType tipoDato;
        public Object valorParametro;
    }
    #endregion

    public static class DAL
    {
        #region Iniciar Conexion con la Base de Datos
        public static SqlConnection traerConexion(string nombreConexion, ref string mensajeError, ref int numError)
        {
            SqlConnection conn;

            try
            {
                string cadenaConexion = "";

                cadenaConexion = ConfigurationManager.ConnectionStrings[nombreConexion].ToString();

                conn = new SqlConnection(cadenaConexion);
                mensajeError = String.Empty;
                numError = 0;
                return conn;
            }
            catch (NullReferenceException ex)
            {
                mensajeError = "NO SE ENCONTRO LA CADENA DE CONEXION: " + nombreConexion + "/nERROR: " + ex.Message;
                numError = -1;
                return null;
            }
            catch (ConfigurationException ex)
            {
                mensajeError = "ERROR: " + ex.Message;
                numError = -2;
                return null;
            }
        }
        #endregion

        #region Conexion con la Base de Datos
        public static void conectar(SqlConnection conn, ref string mensajeError, ref int numError)
        {
            try
            {
                conn.Open();
                mensajeError = "ok";
                numError = 0;
            }
            catch (SqlException ex)
            {
                mensajeError = "ERROR CON LA CONEXION DEL SERVIDOR CON LA BASE DE DATOS. /n ERROR:" + ex.Message;
                numError = ex.Number;
            }
        }
        #endregion

        #region Desconectar la Base de Datos
        public static void desconectar(SqlConnection conn, ref string mensajeError, ref int numError)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    mensajeError = "CONEXION CERRADA";
                    numError = 0;
                }
                else
                {
                    conn.Close();
                    mensajeError = "CONEXION SIGQUE ABEIRTA";
                }
            }
            catch (SqlException ex)
            {
                mensajeError = "ERROR CON LA CONEXION DEL SERVIDOR CON LA BASE DE DATOS. /nERROR" + ex.Message;
                numError = ex.Number;
            }
        }
        #endregion

        #region Ejecuctar Data Set
        public static DataSet ejecutarDataSet(SqlConnection conn, string sql, bool procedimientoAlmacenado, ref string mensajeError, ref int numError)
        {
            SqlDataAdapter dataAdapter;
            DataSet dataSet = new DataSet();
            try
            {
                dataAdapter = new SqlDataAdapter(sql, conn);
                if (procedimientoAlmacenado)
                {
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                }
                dataAdapter.Fill(dataSet);
                numError = 0;
                mensajeError = "ok";
                return dataSet;
            }
            catch (SqlException ex)
            {
                numError = ex.Number;
                mensajeError = ex.Message;
                return null;
            }
        }
        #endregion

        #region Ejecutar Parametros del Data Set
        public static DataSet ejecutarDataSetParametros(SqlConnection conn, string sql, bool procedimientoAlmacenado, Parametros[] parametros, ref string mensajeError, ref int numError)
        {
            SqlDataAdapter dataAdapter;
            DataSet dataSet = new DataSet();
            try
            {
                dataAdapter = new SqlDataAdapter(sql, conn);
                if (procedimientoAlmacenado)
                {
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                }
                foreach (Parametros var in parametros)
                {
                    agregarParametroAdapter(ref dataAdapter, var.nombreParametro, var.valorParametro.ToString(), var.tipoDato);
                }
                dataAdapter.Fill(dataSet);
                numError = 0;
                mensajeError = "ok";
                return dataSet;
            }
            catch (SqlException ex)
            {
                numError = ex.Number;
                mensajeError = ex.Message;
                return null;
            }
        }
        #endregion

        #region Ejecutar Data Reader
        public static void ejecutarDataReader(ref SqlDataReader dataReader, SqlCommand query, ref string mensajeError, ref int numError)
        {
            try
            {
                dataReader = query.ExecuteReader(CommandBehavior.CloseConnection);
                numError = 0;
                mensajeError = "ok";
            }
            catch (SqlException ex)
            {
                numError = ex.Number;
                mensajeError = ex.Message;
            }
        }
        #endregion

        #region Ejecutar Comandos SQL
        public static void ejecutaSqlCommand(SqlConnection conn, string sql, bool procedimientoAlmacenado, ref string mensajeError, ref int numError)
        {
            SqlCommand query;
            try
            {
                query = new SqlCommand(sql, conn);
                if (procedimientoAlmacenado)
                {
                    query.CommandType = CommandType.StoredProcedure;
                }
                int resultado = 0;
                resultado = query.ExecuteNonQuery();
                mensajeError = "ok";
                numError = 0;
            }
            catch (SqlException ex)
            {
                mensajeError = "ERROR AL EJECUTAR LA SENTENCIA DEL SQL. /nERROR: " + ex.Message;
                numError = ex.Number;
            }
        }
        #endregion

        #region Ejecutar Parametros del Comando SQL
        public static void ejecutarSqlCommandParametros(SqlConnection conn, string sql, bool procedimientoAlmacenado, Parametros[] parametros, ref string mensajeError, ref int numError)
        {
            SqlCommand query;
            try
            {
                int resultado = 0;
                query = new SqlCommand(sql, conn);
                if (procedimientoAlmacenado)
                {
                    query.CommandType = CommandType.StoredProcedure;
                }
                foreach (Parametros var in parametros)
                {
                    agregarParametroCommand(ref query, var.nombreParametro, var.valorParametro.ToString(), var.tipoDato);
                }
                resultado = query.ExecuteNonQuery();
                mensajeError = "ok";
                numError = 0;
            }
            catch (SqlException ex)
            {
                mensajeError = "ERROR AL EJECUTAR LA SENTENCIA DEL SQL. /nERROR: " + ex.Message;
                numError = ex.Number;
            }
        }
        #endregion

        #region Agregar Parametros para Comando SQL
        public static void agregarParametroCommand(ref SqlCommand command, string nombreParametro, string valorParametro, SqlDbType tipoDato)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombreParametro;
            parametro.Value = valorParametro;
            parametro.SqlDbType = tipoDato;
            command.Parameters.Add(parametro);
        }
        #endregion

        #region Agregar Parametros para Adapter
        public static void agregarParametroAdapter(ref SqlDataAdapter dataAdapter, string nombreParametro, string valorParametro, SqlDbType tipoDato)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombreParametro;
            parametro.Value = valorParametro;
            parametro.SqlDbType = tipoDato;
            dataAdapter.SelectCommand.Parameters.Add(parametro);
        }
        #endregion

        #region Agregar Estructura de Parametros
        public static void agregarEstructuraParametros(ref Parametros[] parametros, int posicion, string nombreParametro, SqlDbType tipoDato, object valorParametro)
        {
            parametros[posicion].nombreParametro = nombreParametro.ToString();
            parametros[posicion].tipoDato = tipoDato;
            parametros[posicion].valorParametro = valorParametro;
        }
        #endregion
    }
}
