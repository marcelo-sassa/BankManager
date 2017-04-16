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
    public partial class frmDeposito : Form
    {
        public Conta Conta { get; set; }
        DaoConta DaoConta;

        public frmDeposito(int Id)
        {
            InitializeComponent();
            DaoConta = new DaoConta();
            Conta = DaoConta.CarregaUnicaConta(Id);
        }

        private void frmDeposito_FormClosing(object sender, FormClosingEventArgs e)
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
                if (MessageBox.Show("Deseja realmente efetuar o depósito?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Conta.Deposito(Convert.ToDecimal(txtValor.Text));

                    DaoConta.AtualizaConta(Conta);
                    MessageBox.Show("Depósito realizado com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if(txtValor.Text != "")
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

        private void frmDeposito_Load(object sender, EventArgs e)
        {
            txtNumero.Text = Conta.Numero;
            txtAgencia.Text = Conta.Agencia;
            txtSaldo.Text = Conta.Saldo.ToString();
        }
    }
}
