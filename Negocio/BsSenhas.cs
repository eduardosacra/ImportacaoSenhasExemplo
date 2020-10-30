using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class BsSenhas
    {
        public List<SolicitacaoSenha> LeituraSenhasCsv(string diretorioArquivo)
        {
            var listaSolicitacaoSenhas = new List<SolicitacaoSenha>();

            
            this.VerificaDiretorioExiste(diretorioArquivo);

            try
            {
                StreamReader stream = new StreamReader(diretorioArquivo);
                string linha = null;
                stream.ReadLine();
                while ((linha = stream.ReadLine()) != null)
                {
                    string[] linhaSeparada = linha.Split(';');
                    if (linhaSeparada.Length > 0)
                    {
                        var senha = new SolicitacaoSenha();
                        senha.NumeroCartao = linhaSeparada[0];
                        senha.CPF = linhaSeparada[1];
                        senha.Agencia = linhaSeparada[2];
                        senha.Conta = linhaSeparada[3];
                        senha.DataMovimento = DateTime.Parse(linhaSeparada[4]);
                        listaSolicitacaoSenhas.Add(senha);
                    }
                    
                }
                stream.Close();
            }
            catch (Exception erro)
            {
                throw new Exception("Erro ao ler arquivo", erro);
            }

            if (listaSolicitacaoSenhas.Count() == 0)
                throw new Exception("Nenhum Registro Localizado");

            return listaSolicitacaoSenhas;
        }

        private void VerificaDiretorioExiste(string diretorioArquivo)
        {
            if (!File.Exists(diretorioArquivo))
            {
                throw new FileNotFoundException("Arquivo Não encontrado");
            }
        }

        public DataTable RecuperaSenhasCsvToDataTable(string diretorio)
        {

            var dtSenhas = new DataTable();

            try
            {
                StreamReader stream = new StreamReader(diretorio);
                string linha = null;
                var header =  stream.ReadLine();
                string[] cabecalhos = header.Split(';');
                for (int i = 0; i < cabecalhos.Length-1; i++)
                {
                    dtSenhas.Columns.Add(cabecalhos[i], typeof(string));
                }
                dtSenhas.Columns.Add(cabecalhos[cabecalhos.Length - 1], typeof(DateTime));

                while ((linha = stream.ReadLine()) != null)
                {
                    string[] linhaSeparada = linha.Split(';');
                    if (linhaSeparada.Length > 0)
                    {
                        
                        var senha = new SolicitacaoSenha();
                        senha.NumeroCartao = linhaSeparada[0];
                        senha.CPF = linhaSeparada[1];
                        senha.Agencia = linhaSeparada[2];
                        senha.Conta = linhaSeparada[3];
                        senha.DataMovimento = DateTime.Parse(linhaSeparada[4]);

                        dtSenhas.Rows.Add(senha.NumeroCartao, senha.CPF, senha.Agencia, senha.Conta, senha.DataMovimento);
                        
                    }

                }
                stream.Close();

                if (dtSenhas.Rows.Count == 0)
                    throw new Exception("Nenhum Registro Localizado");

            }
            catch (Exception erro)
            {
                throw new Exception("Erro ao ler arquivo");
            }


            return dtSenhas;
        }
    }
}
