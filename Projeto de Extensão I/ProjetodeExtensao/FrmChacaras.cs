using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetodeExtensao
{
    public partial class FrmChacaras : Form
    {
        private CRUDChacara CRUDchacara;
        private DataTable dataTable;
        public FrmChacaras()
        {
            InitializeComponent();
            this.CRUDchacara = new CRUDChacara();
        }

        private void btnInicio2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormInicial);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormInicial()
        {
            Application.Run(new FrmInicial()); // abre o formulário de inicio
        }

        private void btnClientes2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormClientes);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormClientes()
        {
            Application.Run(new FrmClientes());
        }

        private void btnDiarias2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormDiarias);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormDiarias()
        {
            Application.Run(new FrmDiarias());
        }

        private void btnVisitas2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormVisitas);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormVisitas()
        {
            Application.Run(new FrmVisitas());
        }

        private void btnPagamento2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormPagamento);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormPagamento()
        {
            Application.Run(new FrmPagamento());
        }

        private void btnContratos2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormContrato);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormContrato()
        {
            Application.Run(new FrmContratos());
        }

        private void btnSair2_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormLogin);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormLogin()
        {
            Application.Run(new FrmLogin()); // volta para a tela de Login
        }

        private void btnAdicionar0_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormCadastrar);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormCadastrar()
        {
            Application.Run(new FrmCadastrarChacara()); // tela de adicionar
        }

        private void btnExcluir0_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormExcluir);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormExcluir()
        {
            Application.Run(new FrmExcluirChacara()); // tela de excluir
        }

        private void btnAtualizar0_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormAtualizar);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormAtualizar()
        {
            Application.Run(new FrmAtualizarChacara()); // tela de atualizar
        }

        private void FrmChacaras_Load(object sender, EventArgs e)
        {
            // Cria uma tabela vazia
            DataTable dataTable = new DataTable();

            // Instancia o objeto CRUD para trabalhar com as chácaras
            CRUDChacara cadastroChacara = new CRUDChacara();

            // Insere os dados do relatório na tabela
            cadastroChacara.gerarRelatorioChacaras(dataTable);

            // Define a fonte de dados do DataGridView para a tabela gerada
            gvChacaras.DataSource = dataTable;
        }
    }
}
