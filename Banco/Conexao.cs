using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BancoDeDados
{
    class Conexao
    {
        public static SqlConnection GetConexao()
        {
            var stringConexao = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(stringConexao);

            return sqlConn;
        }
    }
}
