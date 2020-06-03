using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql; //Npgsql .NET Data Provider for PostgreSQL

namespace DATA
{
    public class DbPostgresql : IConnection
    {
        protected DataSet myDataSet;
        protected NpgsqlConnection conn;
        protected NpgsqlCommand command;
        protected NpgsqlDataAdapter myAdapter;
        protected NpgsqlDataReader myReader;

        private string host = "0.0.0.0";
        private string db = "bdName";
        private string user = "postgres";
        private string pass = "pass";

        public void Conectar()
        {
            Console.WriteLine("Conectando : Postgres");
            try
            {
                conn = new NpgsqlConnection();
                command = new NpgsqlCommand();
                myAdapter = new NpgsqlDataAdapter();
                myDataSet = new DataSet();

                string MyConnectionString = "Server=" + host + ";" +
                                    "Database=" + db + ";User ID=" + user + ";Password=" + pass + ";";
                conn.ConnectionString = MyConnectionString;
                conn.Open();
            }
            catch (NpgsqlException ex)
            {
                Desconectar();
                throw new Exception("Error al conectarse a " + host + " | detalle: " + ex.Message);
            }
        }

        public void Desconectar()
        {
            Console.WriteLine("Desconectando : Postgres");
            try
            {
                command.Dispose();
                myAdapter.Dispose();
                conn.Close();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                throw new Exception("Error al intentar desconectarse a " + host + " | detalle: " + ex.Message);
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
            catch (NpgsqlException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta postgreSql | detalle: " + ex.Message);
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                Conectar();
                using (var c = conn)
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
            catch (NpgsqlException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta postgreSql| detalle: " + ex.Message);
            }
        }

        public void ExecuteSP(string name_sp, parametros p)
        {
            Conectar();
            try
            {
                using (var c = conn)
                {
                    command = new NpgsqlCommand(name_sp, c);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60 * 10; //10 minutos
                    for (int i = 0; i < p.Count(); i++)
                    {
                        NpgsqlTypes.NpgsqlDbType param;
                        param = GetDbType(p.GetTypeData(i));

                        command.Parameters.Add("@" + p.GetName(i), param).Value = p.GetValue(i);
                    }
                    command.ExecuteNonQuery();
                    Desconectar();
                }
                //comando = new SqlCommand(name_sp, conn);
            }
            catch (NpgsqlException ex)
            {
                Desconectar();
                throw new Exception("Error al ejecutar la consulta | detalle: " + ex.Message);
            }
        }

        private static NpgsqlTypes.NpgsqlDbType GetDbType(parametros.eTypeData o)
        {
            if (o == parametros.eTypeData.eVarchar) return NpgsqlTypes.NpgsqlDbType.Varchar;
            if (o == parametros.eTypeData.eDateTime) return NpgsqlTypes.NpgsqlDbType.Timestamp;
            if (o == parametros.eTypeData.eInt) return NpgsqlTypes.NpgsqlDbType.Integer;
            if (o == parametros.eTypeData.eLong) return NpgsqlTypes.NpgsqlDbType.Bigint;
            if (o == parametros.eTypeData.eBool) return NpgsqlTypes.NpgsqlDbType.Boolean;
            if (o == parametros.eTypeData.eDecimal) return NpgsqlTypes.NpgsqlDbType.Money;
            if (o == parametros.eTypeData.eDouble) return NpgsqlTypes.NpgsqlDbType.Double;
            if (o == parametros.eTypeData.eFloat) return NpgsqlTypes.NpgsqlDbType.Real;
            if (o == parametros.eTypeData.eByte) return NpgsqlTypes.NpgsqlDbType.Bytea;

            return NpgsqlTypes.NpgsqlDbType.Varchar;
        }

        /// <summary>
        /// Transforma a un DataReader un DATATABLE
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        protected DataTable GetTable(NpgsqlDataReader _reader)
        {
            DataTable dt = new DataTable();
            dt.Load(_reader);
            return dt;
        }
    }
}