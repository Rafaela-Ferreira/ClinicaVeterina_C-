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
    public partial class FrmClientes : Form
    {
        private CRUDChacara CRUDchacara; //add now
        private DataTable dataTable; //add now

        public FrmClientes()
        {
            InitializeComponent();

            this.CRUDchacara = new CRUDChacara(); //add now
        }

        private void btnInicio1_Click(object sender, EventArgs e)
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

        private void btnChacaras1_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread nt = new Thread(novoFormChacaras);
            nt.SetApartmentState(ApartmentState.STA);
            nt.Start();
        }

        private void novoFormChacaras()
        {
            Application.Run(new FrmChacaras()); // abre o formulário de chacaras
        }

        private void btnDiarias1_Click(object sender, EventArgs e)
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

        private void btnVisitas1_Click(object sender, EventArgs e)
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

        private void btnPagamento1_Click(object sender, EventArgs e)
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

        private void btnContrato1_Click(object sender, EventArgs e)
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

        private void btnSair1_Click(object sender, EventArgs e)
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
            Application.Run(new FrmCadastrarCliente()); // tela de adicionar
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
            Application.Run(new FrmExcluirCliente()); // tela de excluir
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
            Application.Run(new FrmAtualizarCliente()); // tela de atualizar
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            //add now

            // Cria uma tabela vazia
            DataTable dataTable2 = new DataTable();

            // Instancia o objeto CRUD para trabalhar com as chácaras
            CRUDChacara mostrarCliente = new CRUDChacara();

            // Insere os dados do relatório na tabela
            mostrarCliente.mostrarClientes(dataTable2);

            // Define a fonte de dados do DataGridView para a tabela gerada
            gvClientes.DataSource = dataTable2;
        }
    }
}
