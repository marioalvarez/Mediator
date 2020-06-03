using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace DATA
{
    public class DbOracle : IConnection
    {
        protected OracleConnection conn;
        protected OracleCommand command;
        protected OracleDataAdapter myAdapter;
        protected OracleDataReader myReader;

        //Connection Fields
        private string host = "0.0.0.0";
        private string db = "nameDB";
        private string user = "nameUser";
        private string pass = "pass";
        private string port = "1521";

        public void Conectar()
        {
            Console.WriteLine("Conectando: Oracle");
            try
            {
                if (conn == null)
                {
                    conn = new OracleConnection();
                    string MyConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + port + ")))(CONNECT_DATA=(SERVICE_NAME=" + db + ")));User ID=" + user + ";Password=" + pass + ";";
                    conn.ConnectionString = MyConnectionString;
                    conn.Open();
                }
            }
            catch (OracleException ex)
            {
                Desconectar();
                throw new Exception("Error al conectarse a :" + host + "| detalle: " + ex.Message);
            }
        }

        public void Desconectar()
        {
            Console.WriteLine("Desconectando : Oracle");
            try
            {
                conn.Close();
                conn.Dispose();
            }
            catch (OracleException ex)
            {

                throw new Exception("Error al intentar desconectar | Detalle : " + ex.Message);
            }
        }

        public DataTable ExcecuteQuery(string sql)
        {
            DataTable dt = null;
            try
            {
                Conectar();
                command = conn.CreateCommand();
                command.CommandText = sql;
                myReader = command.ExecuteReader();
                dt = GetTable(myReader);
                Desconectar();
                return dt;
            }
            catch (OracleException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta | detalle: " + ex.Message);
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                Conectar();
                using( var c = conn)
                {
                    using (var command = c.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta | detalle: " + ex.Message);
            }
        }

        public void ExecuteSP(string name_sp, parametros p)
        {
            Conectar();
            try
            {
                using(var c = conn)
                {
                    command = new OracleCommand(name_sp, c);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60 * 10;// 10 minuts
                    for (int i = 0; i < p.Count(); i++)
                    {
                        OracleDbType param;
                        param = GetDbType(p.GetTypeData(i));

                        command.Parameters.Add("@" + p.GetName(i), param).Value = p.GetValue(i);
                    }
                    command.ExecuteNonQuery();
                    Desconectar();
                }
            }
            catch (OracleException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta | detalle : " + ex.Message);
            }
        }

        private static OracleDbType GetDbType(parametros.eTypeData o)
        {
            if (o == parametros.eTypeData.eVarchar) return OracleDbType.Varchar2;
            if (o == parametros.eTypeData.eDateTime) return OracleDbType.Date;
            if (o == parametros.eTypeData.eLong) return OracleDbType.Long;
            if (o == parametros.eTypeData.eInt) return OracleDbType.Int32;
            if (o == parametros.eTypeData.eBool) return OracleDbType.Long;
            if (o == parametros.eTypeData.eDecimal) return OracleDbType.Decimal;
            if (o == parametros.eTypeData.eFloat) return OracleDbType.Single;
            if (o == parametros.eTypeData.eDouble) return OracleDbType.Double;
            if (o == parametros.eTypeData.eByte) return OracleDbType.Blob;

            return OracleDbType.Varchar2;
         }

        /// <summary>
        /// Transforma un Datareader un Datable
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        protected DataTable GetTable(OracleDataReader _reader)
        {
            DataTable dt = new DataTable();
            dt.Load(_reader);
            return dt;
        }
    }
}
