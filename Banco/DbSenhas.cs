using Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BancoDeDados
{
    public class DbSenhas
    {
        public List<SolicitacaoSenha> GetSolicitacaoSenhas(DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                

                var lista = new List<SolicitacaoSenha>();
                var stringConexao = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
                var sqlConn = new SqlConnection(stringConexao);
                using (System.Data.SqlClient.SqlConnection conn = sqlConn)
                {
                    conn.Open();

                    try
                    {
                        var query = "select * from BaseConsolidada where DataMovimento >= @DataInicial and DataMovimento <= @DataFinal";

                        using (var commad = new System.Data.SqlClient.SqlCommand(query, conn))
                        {
                            commad.Parameters.AddWithValue("@DataInicial", dtInicio.ToString("yyyy-MM-dd"));
                            commad.Parameters.AddWithValue("@DataFinal", dtFim.ToString("yyyy-MM-dd"));

                            var dr = commad.ExecuteReader();
                            while (dr.Read())
                            {
                                var solicitacaoSenha = new SolicitacaoSenha();

                                solicitacaoSenha.NumeroCartao = dr.GetString(0);
                                solicitacaoSenha.CPF = dr.GetString(1);
                                solicitacaoSenha.Agencia = dr.GetString(2);
                                solicitacaoSenha.Conta = dr.GetString(3);
                                if (!dr.IsDBNull(4))
                                    solicitacaoSenha.DataMovimento = dr.GetDateTime(4);
                                solicitacaoSenha.Id = dr.GetInt32(5);

                                lista.Add(solicitacaoSenha);
                            }
                        }
                    }
                    catch (Exception erro)
                    {
                        conn.Close();
                        throw new Exception($"Falha ao Consultar o Banco {erro.Message}", erro);
                    }
                }

                return lista;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }

        }

        public bool insertEmMassaSenhas(DataTable dtSenhas)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            using (SqlConnection destinationConnection =
                     new SqlConnection(connectionString))

            {
                destinationConnection.Open();

                
                using (SqlBulkCopy bulkCopy =
                           new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.DestinationTableName =
                        "dbo.BaseConsolidada";

                    try
                    {
                        bulkCopy.WriteToServer(dtSenhas);
                    }
                    catch (Exception ex)
                    {
                       throw new Exception(ex.Message);
                    }
                    finally
                    {
                     
                        destinationConnection.Close();
                    }
                }
            }
            return true;
        }
    }
}
