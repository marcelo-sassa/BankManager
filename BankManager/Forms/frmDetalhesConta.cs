using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankManager.Forms;

namespace BankManager
{
    public partial class frmDetalhesConta : Form
    {
        public Conta Conta { get; set; }
        DaoConta DaoConta;

        public frmDetalhesConta(Conta c)
        {
            InitializeComponent();
            DaoConta = new DaoConta();
            Conta = c;
        }

        private void DetalhesConta_Load(object sender, EventArgs e)
        {
            txtId.Text = Conta.Id.ToString();
            txtNumero.Text = Conta.Numero;
            txtAgencia.Text = Conta.Agencia;
            if (Conta.Tipo.Equals("C"))
            {
                rdbCorrente.Checked = true;
            }
            else
            {
                rdbPoupanca.Checked = true;
            }
            txtSaldo.Text = Conta.Saldo.ToString();
        }

        private void DetalhesConta_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmContas frmContas = new frmContas();
            frmContas.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidaCampos())
                {
                    return;
                }
                if (MessageBox.Show("Confirma as alterções na conta?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Conta.Agencia = txtAgencia.Text;
                    Conta.Tipo = rdbCorrente.Checked == true ? "C" : "P";

                    DaoConta.AtualizaConta(Conta);
                    MessageBox.Show("Conta atualizada com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Refresh();
                }
                else
                {
                    txtAgencia.Text = Conta.Agencia;
                    if (Conta.Tipo.Equals("C"))
                    {
                        rdbCorrente.Checked = true;
                    }
                    else
                    {
                        rdbPoupanca.Checked = true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAgencia_TextChanged(object sender, EventArgs e)
        {

            Int32 valorConvertido;

            if (!int.TryParse(txtAgencia.Text, out valorConvertido))
            {
                if (txtAgencia.Text != "")
                {
                    MessageBox.Show("O campo Agência aceita apenas números!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtAgencia.Text = "";
            }
        }

        public bool ValidaCampos()
        {
            if (txtAgencia.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Preencha a Agência", "Campo obrigatório!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAgencia.Focus();
                return false;
            }
            if (txtAgencia.Text.Length > 10)
            {
                MessageBox.Show("O campo Agência deve ter no máximo 10 caracteres", "Campo obrigatório!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAgencia.Focus();
                return false;
            }
            return true;
        }

        private void btnSaque_Click(object sender, EventArgs e)
        {
            frmSaque frmSaque = new frmSaque(Conta.Id);
            Hide();
            frmSaque.Show();
        }

        private void btnDeposito_Click(object sender, EventArgs e)
        {
            frmDeposito frmDeposito = new frmDeposito(Conta.Id);
            Hide();
            frmDeposito.Show();
        }

        private void btnTransferencia_Click(object sender, EventArgs e)
        {
            frmTransferencia frmTransferencia = new frmTransferencia(Conta.Id);
            Hide();
            frmTransferencia.Show();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
