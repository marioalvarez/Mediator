using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    /// <summary>
    /// Pattern Fabric
    /// </summary>
    public class ConnectionFactory
    {
        public enum eTypeEngine
        {
            Oracle = 1,
            SqlServer = 2,
            Postgresql = 3
        }

        public IConnection GetConnection(eTypeEngine engine)
        {
            switch (engine)
            {
                case eTypeEngine.Oracle:
                    Console.WriteLine("Conectado al motor de Oracle 11g");
                    return new DbOracle();

                case eTypeEngine.SqlServer:
                    Console.WriteLine("Conectado al motor de Sql Server");
                    return new DbSqlServer();

                case eTypeEngine.Postgresql:
                    Console.WriteLine("Conectado al motor de PostgreSQL");
                    return new DbPostgresql();

                default:
                    Console.WriteLine("motor no especificado - Revisar*");
                    return null;
            }
        }
    }
}
