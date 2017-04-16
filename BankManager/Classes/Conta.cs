using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManager
{
    public class Conta
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Agencia { get; set; }
        public string Tipo { get; set; }
        public decimal Saldo { get; set; }
        public string NumAg { get { return "Nº " + Numero + " - Ag. " + Agencia;} }

        public decimal Deposito(decimal valor)
        {
            Saldo += valor;
            return Saldo;
        }

        public virtual decimal Saque(decimal valor)
        {
            Saldo -= valor;
            return Saldo;
        }
    }
}
