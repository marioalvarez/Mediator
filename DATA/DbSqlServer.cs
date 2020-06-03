using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    public class DbSqlServer : IConnection
    {
        protected DataSet myDataSet;
        protected SqlConnection conn;
        protected SqlCommand command;
        protected SqlDataAdapter myAdapter;
        protected SqlDataReader myReader;

        private string host = @"WIN-U1MBPRKR5L6\SQLEXPRESS";
        private string db = "nombre_db";
        private string user = "sa";
        private string pass = "pass";


        public void Conectar()
        {
            Console.WriteLine("Conectando a: SQl Server");
            try
            {
                conn = new SqlConnection();
                command = new SqlCommand();
                myAdapter = new SqlDataAdapter();
                myDataSet = new DataSet();

                string MyConnectionString = "Server=" + host + ";" +
                                            "Database=" + db + ";User ID=" + user +
                                            ";Password=" + pass + ";" +
                                            "Pooling=true;" +
                                            "Min Pool Size=0;" +
                                            "Max Pool Size=100;" +
                                            "Connection Lifetime=0";
                conn.ConnectionString = MyConnectionString;
                conn.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Desconectar();
                throw new Exception("Error al conectar a " + host +"Detalle: " + ex.Message);
            }
        }

        public void Desconectar()
        {
            Console.WriteLine("Desconectando de: Sql Server");
            try
            {
                command.Dispose();
                myAdapter.Dispose();
                conn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al intentar desconectar | detalle: " + ex.Message);
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
            catch (SqlException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta sql | Detalle: " + ex.Message); 
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                Conectar();
                using(var c = conn)
                {
                    using (var command = c.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
                Desconectar();
            }
            catch (SqlException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta SQL | Detalle: " + ex.Message);
            }
        }

        public void ExecuteSP(string name_sp, parametros p)
        {
            Conectar();
            try
            {
                command = new SqlCommand(name_sp, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60 * 10; //10 minutos

                for (int i = 0; i < p.Count(); i++)
                {
                    SqlDbType param;
                    param = GetDbType(p.GetTypeData(i));

                    command.Parameters.Add("@" + p.GetName(i), param).Value = p.GetValue(i);
                }

                command.ExecuteNonQuery();
                Desconectar();
            }
            catch (SqlException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta | detalle: " + ex.Message);
            }
        }

        private static SqlDbType GetDbType(parametros.eTypeData o)
        {
            if (o == parametros.eTypeData.eVarchar) return SqlDbType.VarChar;
            if (o == parametros.eTypeData.eDateTime) return SqlDbType.DateTime;
            if (o == parametros.eTypeData.eInt) return SqlDbType.Int;
            if (o == parametros.eTypeData.eLong) return SqlDbType.BigInt;
            if (o == parametros.eTypeData.eBool) return SqlDbType.Bit;
            if (o == parametros.eTypeData.eDecimal) return SqlDbType.Decimal;
            if (o == parametros.eTypeData.eDouble) return SqlDbType.Float;
            if (o == parametros.eTypeData.eFloat) return SqlDbType.Real;
            if (o == parametros.eTypeData.eByte) return SqlDbType.Image;

            return SqlDbType.VarChar;
        }

        /// <summary>
        /// Transforma a un DataReader un DATATABLE
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        protected DataTable GetTable(SqlDataReader _reader)
        {
            DataTable dt = new DataTable();
            dt.Load(_reader);
            return dt;
        }
    }
}
