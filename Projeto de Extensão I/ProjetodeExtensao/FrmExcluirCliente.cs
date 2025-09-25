using MySql.Data.MySqlClient;
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
    public partial class FrmExcluirCliente : Form
    {
        public FrmExcluirCliente()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
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

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                // Recupera o CPF usado na pesquisa e remove os espaços em branco do começo e fim
                string cpf = txbPesquisa.Text.Trim();
                if (string.IsNullOrEmpty(cpf))
                {
                    Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"CPF\"");
                    return;
                }

                // Cria uma instância do CRUDCliente
                CRUDCliente crudCliente = new CRUDCliente();

                // Pesquisa se existe um cliente com o CPF fornecido
                Cliente cliente = crudCliente.pesquisarClientePorCPF(cpf);

                // Caso encontre um cliente, a referência será diferente de null
                if (cliente != null)
                {
                    // Preenche os campos com os dados do cliente encontrado
                    txbNome.Text = cliente.GetNome();
                    txbCPF.Text = cliente.GetCPF();
                    txbEmail.Text = cliente.GetEmail();
                    txbSexo.Text = cliente.GetSexo();
                    // Preencha outros campos conforme necessário
                    Endereco endereco = cliente.GetEndereco();
                    

                    // Preenche cada TextBox com as informações do endereço
                    txbRua.Text = endereco.GetRua();
                    txbNumero.Text = endereco.GetNumero().ToString();
                    txbBairro.Text = endereco.GetBairro();
                    txbCidade.Text = endereco.GetCidade();
                    txbCEP.Text = endereco.GetCEP();
                    txbComplemento.Text = endereco.GetComplemento();
                }
                else
                {
                    Ferramentas.mostraErroTextBox(txbPesquisa, "Não existe um cliente com esse CPF!");
                }
            }
            catch (MySqlException erro)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(erro.GetType().ToString());
                sb.AppendLine(erro.Message);
                sb.Append(erro.SqlState);
                sb.AppendLine("\n");
                sb.AppendLine(erro.StackTrace);
                MessageBox.Show(sb.ToString(), "ERRO BANCO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception erro)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(erro.GetType().ToString());
                sb.AppendLine(erro.Message);
                sb.AppendLine("\n");
                sb.AppendLine(erro.StackTrace);
                MessageBox.Show(sb.ToString(), "ERRO Desconhecido!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Limpa os TextBox de dados
        private void clearTextBox()
        {
            txbNome.Clear();
            txbCPF.Clear();
            txbEmail.Clear();
            txbSexo.Clear();
            txbRua.Clear();
            txbNumero.Clear();
            txbBairro.Clear();
            txbCidade.Clear();
            txbCEP.Clear();
            txbComplemento.Clear();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Recupera o CPF usado na pesquisa e remove os espaços em branco do começo e fim
            string cpfCliente = txbPesquisa.Text.Trim();

            if (cpfCliente == "")
            {
                Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"CPF - Pesquisar\"");
                return;
            }

            // Instancia o objeto de CRUD para Cliente
            CRUDCliente crudCliente = new CRUDCliente();

            // Caso encontre um cliente com o CPF digitado e consiga removê-lo
            if (crudCliente.removerCliente(cpfCliente) == true)
            {
                // Limpa os TextBoxes com os dados do cliente
                clearTextBox();

                // Informa ao usuário que o cliente foi removido com sucesso
                MessageBox.Show("Cliente removido do cadastro com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Caso não encontre um cliente com o CPF digitado ou ocorra um erro na remoção
            else
            {
                // Limpa os TextBoxes com os dados do cliente
                clearTextBox();

                // Exibe uma mensagem de erro informando que a remoção falhou
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Não foi possível remover o cliente do cadastro!");
                sb.AppendLine("Verifique se existe um cliente cadastrado com o CPF pesquisado!");
                Ferramentas.mostraErroTextBox(txbPesquisa, sb.ToString());
            }

            // Limpa os TextBoxes de dados
            clearTextBox();
        }
    }
}
