using BancoDeDados;
using Modelo;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportacaoSenhas
{
    public partial class Principal : Form
    {
        private BsSenhas _bsSenha;
        private DbSenhas _bdSenhas;
        public Principal()
        {
            InitializeComponent();
            this._bsSenha = new BsSenhas();
            this._bdSenhas = new DbSenhas();
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {

            var consulta = new RegrasConsulta();

            //var listaSenhas = new List<SolicitacaoSenha>() { 
            //    new SolicitacaoSenha() { Agencia = "1223", Conta = "123456", CPF="12312312312", NumeroCartao= "1234123412341234", DataMovimento = DateTime.Now} 
            //};

            //var result = consulta.teste();
            try
            {
                var listaSenhas = consulta.GetSolicitacaoSenhas(this.dtpInicial.Value, this.dtpFinal.Value);
                this.dataGridView1.DataSource = listaSenhas;

                var stringConexao = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            }
            catch (Exception erro)
            {
                MessageBox.Show($"Erro durante a consulta: {erro.Message}");
            }

        }

        private void btnSelecionarArquivo_Click(object sender, EventArgs e)
        {
            var diretorioSelecionado = SelecionaArquivo();
            if (diretorioSelecionado != "" && File.Exists(diretorioSelecionado))
            {
                this.txtDiretorioArquivoSelecionado.Text = diretorioSelecionado;
            }
        }

        private string SelecionaArquivo()
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName; 
                }

                return filePath;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var diretorioSelecionado = this.txtDiretorioArquivoSelecionado.Text;

            if (!File.Exists(diretorioSelecionado))
            {
                MessageBox.Show("Selecione um Arquivo para Realizar a Importação", "Selecione um Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    //List<SolicitacaoSenha> listaSenha = this._bsSenha.LeituraSenhasCsv(diretorioSelecionado);
                    DataTable dtSenhas = this._bsSenha.RecuperaSenhasCsvToDataTable(diretorioSelecionado);
                    if(dtSenhas.Rows.Count == 0)
                    {
                        MessageBox.Show("Nenhum Registro Encontrado para fazer a importção, conferir Arquivo selecionado");
                    }else
                    {
                        try
                        {
                            this._bdSenhas.insertEmMassaSenhas(dtSenhas);
                            MessageBox.Show("Importação Realizada com sucesso");
                            this.txtDiretorioArquivoSelecionado.Text = "";
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show($"Erro durante a Insersão no Banco de dados, confira conexão na rede e funcionamento de Rede e disponibilidade do Banco de Dados, detalhes: {erro.Message}");
                        }
                        
                        
                    }
                    
                }
                catch (Exception error)
                {
                    var innerException = "";
                    if (error.InnerException != null)
                    {
                        innerException = error.InnerException.Message;
                    }
                    MessageBox.Show($"Erro durante a importação, {error.Message} {innerException}", "Erro no processamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            
        }
    }
}
