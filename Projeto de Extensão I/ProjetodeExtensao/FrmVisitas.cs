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
    public partial class FrmVisitas : Form
    {
        public FrmVisitas()
        {
            InitializeComponent();
        }

        private void btnInicio4_Click(object sender, EventArgs e)
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

        private void btnChacaras4_Click(object sender, EventArgs e)
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

        private void btnClientes4_Click(object sender, EventArgs e)
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

        private void btnDiarias4_Click(object sender, EventArgs e)
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

        private void btnPagamento4_Click(object sender, EventArgs e)
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

        private void btnContratos4_Click(object sender, EventArgs e)
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

        private void btnSair4_Click(object sender, EventArgs e)
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

    }
}
