using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportacaoSenhas
{
    public partial class Login : Form
    {
        string usuario;
        public Login()
        {
            InitializeComponent();
            this.usuario = "Digite Um Nome";
        }
        
        private void btnLogar_Click(object sender, EventArgs e)
        {
            var usuario = this.tbUsuario.Text;

            var senha = this.tbSenha.Text;

            if (usuario.Count() == 0 || senha.Count() == 0)
            {
                MessageBox.Show("Usuário ou senha Incorretos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else
            {
                var formPrincipal = new Principal();
                formPrincipal.Show();
                
            }
        }
    }
}
