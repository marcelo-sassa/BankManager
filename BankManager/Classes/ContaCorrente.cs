using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManager
{
    public class ContaCorrente : Conta
    {

        public override decimal Saque(decimal valor)
        {
            Saldo -= (valor + 0.20m);
            return Saldo;
        }

    }
}
