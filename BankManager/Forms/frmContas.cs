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
    public partial class frmContas : Form
    {
        DaoConta DaoConta;

        public frmContas()
        {
            InitializeComponent();
            DaoConta = new DaoConta();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtgContas.DataSource = DaoConta.CarregaContas();
        }

        public void CarregaDetalhes()
        {
            Conta linha = dtgContas.CurrentRow.DataBoundItem as Conta;

            frmDetalhesConta detalhes = new frmDetalhesConta(linha);
            Hide();
            detalhes.Show();
        }

        private void dtgContas_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CarregaDetalhes();
        }

        private void dtgContas_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CarregaDetalhes();
        }

        private void btnNovaConta_Click(object sender, EventArgs e)
        {
            frmCadastroConta frmCadastroConta = new frmCadastroConta();
            Hide();
            frmCadastroConta.Show();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtgContas.SelectedRows.Count <= 0)
                {
                    throw new Exception("É necessário selecionar pelo menos uma conta para excluir");
                }
                else if (dtgContas.SelectedRows.Count > 1)
                {
                    throw new Exception("Selecione apenas uma conta");
                }
                if (MessageBox.Show("Deseja realmente excluir esta conta?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridViewRow linha = dtgContas.SelectedRows[0];
                    int Id = Convert.ToInt32(linha.Cells["Id"].Value);
                    DaoConta.ExcluiConta(Id);
                    MessageBox.Show("Conta excluída com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dtgContas.DataSource = DaoConta.CarregaContas();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmContas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
