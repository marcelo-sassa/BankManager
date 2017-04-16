using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManager.Forms
{
    public partial class frmSaque : Form
    {
        public Conta Conta { get; set; }
        DaoConta DaoConta;

        public frmSaque(int Id)
        {
            InitializeComponent();
            DaoConta = new DaoConta();
            Conta = DaoConta.CarregaUnicaConta(Id);
        }

        private void frmSaque_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmDetalhesConta frmDetalhesConta = new frmDetalhesConta(Conta);
            frmDetalhesConta.Show();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidaCampos())
                {
                    return;
                }

                var valor = Convert.ToDecimal(txtValor.Text);

                if (Conta.Tipo == "P" && valor > 1000)
                {
                    throw new Exception("Valor de saque limitado a R$1000,00 para conta Poupança");
                }
                if(Conta.Saldo < valor)
                {
                    throw new Exception("O valor do saque excede o saldo disponível");
                }
                if (MessageBox.Show("Deseja realmente efetuar o saque?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Conta.Saque(valor);

                    DaoConta.AtualizaConta(Conta);
                    MessageBox.Show("Saque realizado com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtValor_TextChanged(object sender, EventArgs e)
        {

            decimal valorConvertido;

            if (!decimal.TryParse(txtValor.Text, out valorConvertido))
            {
                if (txtValor.Text != "")
                {
                    MessageBox.Show("O campo Valor aceita apenas números, ponto e vírgula!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtValor.Text = "";
            }
        }

        public bool ValidaCampos()
        {
            if (txtValor.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Preencha o Valor", "Campo obrigatório!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtValor.Focus();
                return false;
            }
            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSaque_Load(object sender, EventArgs e)
        {
            txtNumero.Text = Conta.Numero;
            txtAgencia.Text = Conta.Agencia;
            txtSaldo.Text = Conta.Saldo.ToString();
        }
    }
}
