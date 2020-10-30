using System;

namespace Modelo
{
    public class SolicitacaoSenha
    {
        public int Id { get; set; }
        public string NumeroCartao { get; set; }
        public string CPF { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public DateTime DataMovimento { get; set; }
    }
}
