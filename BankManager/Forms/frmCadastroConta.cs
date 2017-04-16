using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManager
{
    public partial class frmCadastroConta : Form
    {
        public Conta Conta { get; set; }
        DaoConta DaoConta;

        public frmCadastroConta()
        {
            InitializeComponent();
            DaoConta = new DaoConta();
            Conta = new Conta();
        }

        private void CadastroConta_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmContas frmContas = new frmContas();
            frmContas.Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidaCampos())
                {
                    return;
                }
                if (MessageBox.Show("Deseja realmente cadastrar esta conta?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Conta.Numero = txtNumero.Text;
                    Conta.Agencia = txtAgencia.Text;
                    Conta.Tipo = rdbCorrente.Checked == true ? "C" : "P";
                    Conta.Saldo = Convert.ToDecimal(txtSaldo.Text);

                    DaoConta.InsereConta(Conta);
                    MessageBox.Show("Conta cadastrada com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception ex)
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
                    MessageBox.Show("Este campo aceita apenas números!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtAgencia.Text = "";
            }
        }

        private void txtNumero_TextChanged(object sender, EventArgs e)
        {
            Int32 valorConvertido;

            if (!int.TryParse(txtNumero.Text, out valorConvertido))
            {
                if (txtNumero.Text != "")
                {
                    MessageBox.Show("Este campo aceita apenas números!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtNumero.Text = "";
            }
        }

        private void txtSaldo_TextChanged(object sender, EventArgs e)
        {
            decimal valorConvertido;

            if (!decimal.TryParse(txtSaldo.Text, out valorConvertido))
            {
                if (txtSaldo.Text != "")
                {
                    MessageBox.Show("O campo Saldo aceita apenas números, ponto e vírgula!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                txtSaldo.Text = "";
            }
        }

        public bool ValidaCampos()
        {
            if (txtNumero.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Preencha o Número", "Campo obrigatório!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNumero.Focus();
                return false;
            }
            if (txtNumero.Text.Length > 15)
            {
                MessageBox.Show("O campo Número deve ter no máximo 15 caracteres", "Campo obrigatório!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNumero.Focus();
                return false;
            }
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
            if (txtSaldo.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Preencha o Saldo", "Campo obrigatório!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSaldo.Focus();
                return false;
            }
            return true;
        }
    }
}
