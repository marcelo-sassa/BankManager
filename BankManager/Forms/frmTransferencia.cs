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
    public partial class frmTransferencia : Form
    {
        public Conta ContaOrigem { get; set; }
        DaoConta DaoConta;

        public frmTransferencia(int Id)
        {
            InitializeComponent();
            DaoConta = new DaoConta();
            ContaOrigem = DaoConta.CarregaUnicaConta(Id);
        }

        private void frmTransferencia_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmDetalhesConta frmDetalhesConta = new frmDetalhesConta(ContaOrigem);
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

                Conta ContaDestino = DaoConta.CarregaUnicaConta(Convert.ToInt32(cboContaDestino.SelectedValue));

                var valor = Convert.ToDecimal(txtValor.Text);

                if (ContaOrigem.Id == ContaDestino.Id)
                {
                    throw new Exception("Selecione como destino uma Conta diferente da Conta de origem");
                }
                if(ContaOrigem.Saldo < valor)
                {
                    throw new Exception("O valor a ser debitado da conta origem excede o saldo disponível");
                }
                if (MessageBox.Show("Deseja realmente efetuar a transferência?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ContaOrigem.Saque(valor);
                    ContaDestino.Deposito(valor);

                    DaoConta.AtualizaConta(ContaOrigem);
                    DaoConta.AtualizaConta(ContaDestino);
                    MessageBox.Show("Transferência realizada com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btncCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmTransferencia_Load(object sender, EventArgs e)
        {
            txtNumero.Text = ContaOrigem.Numero;
            txtAgencia.Text = ContaOrigem.Agencia;
            txtSaldo.Text = ContaOrigem.Saldo.ToString();
            cboContaDestino.DataSource = DaoConta.CarregaContas();
            cboContaDestino.ValueMember = "Id";
            cboContaDestino.DisplayMember = "NumAg";
        }
    }
}
