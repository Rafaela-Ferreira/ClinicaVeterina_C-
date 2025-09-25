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
    public partial class FrmContratos : Form
    {
        public FrmContratos()
        {
            InitializeComponent();
        }

        private void btnInicio6_Click(object sender, EventArgs e)
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

        private void btnChacaras6_Click(object sender, EventArgs e)
        {
            this.Close(); // fecha o FrmInicial
            Thread nt = new Thread(novoFormChacaras); // inicia a thread para abrir o novo form
            nt.SetApartmentState(ApartmentState.STA); // estado que a Thread vai assumir antes de ser iniciada
            nt.Start(); // inicia a thread
        }
        private void novoFormChacaras()
        {
            Application.Run(new FrmChacaras()); // abre o formulário de chacaras
        }

        private void btnClientes6_Click(object sender, EventArgs e)
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

        private void btnDiarias6_Click(object sender, EventArgs e)
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

        private void btnVisitas6_Click(object sender, EventArgs e)
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

        private void btnPagamento6_Click(object sender, EventArgs e)
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

        private void btnSair6_Click(object sender, EventArgs e)
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

        private void FrmContratos_Load(object sender, EventArgs e)
        {
            // Cria uma tabela vazia
            DataTable dataTable3 = new DataTable();

            // Instancia o objeto CRUD para trabalhar com as chácaras
            CRUDChacara mostrarCliente = new CRUDChacara();

            // Insere os dados do relatório na tabela
            mostrarCliente.mostrarContratos(dataTable3);

            // Define a fonte de dados do DataGridView para a tabela gerada
            gvContratos.DataSource = dataTable3;
        }
    }
}
