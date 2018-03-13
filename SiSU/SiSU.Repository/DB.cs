using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace SiSU.Repository
{
    public class DB
    {
        public static Func<string, DbConnection> ConnectionFactory = (connectionString) => new SqlConnection(connectionString);
    }
}
