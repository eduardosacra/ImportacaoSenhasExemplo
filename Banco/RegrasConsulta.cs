using BancoDeDados;
using Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class RegrasConsulta
    {
        public List<SolicitacaoSenha> GetSolicitacaoSenhas(DateTime dtInicio, DateTime dtFim)
        {
            var dbSenhas = new DbSenhas();

            return dbSenhas.GetSolicitacaoSenhas(dtInicio, dtFim);           
            
        }

    }
}
