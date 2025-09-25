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
    public partial class FrmAtualizarCliente : Form
    {
        public FrmAtualizarCliente()
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

        private void btnAdicionar0_Click(object sender, EventArgs e)
        {
            try
            {
                // Recupera e valida o CPF do cliente a ser pesquisado
                string cpfClienteOriginal = txbCPF.Text.Trim();
                if (string.IsNullOrEmpty(cpfClienteOriginal))
                {
                    Ferramentas.mostraErroTextBox(txbCPF, "Insira um valor para o campo \"CPF do Cliente - Pesquisar\"");
                    return;
                }

                // Recupera e valida o nome do cliente
                string nome = txbNome.Text.Trim();
                if (string.IsNullOrEmpty(nome))
                {
                    Ferramentas.mostraErroTextBox(txbNome, "Insira um valor para o campo \"Nome\"");
                    return;
                }

                // Recupera e valida o CPF do cliente
                string cpf = txbCPF.Text.Trim();
                if (string.IsNullOrEmpty(cpf))
                {
                    Ferramentas.mostraErroTextBox(txbCPF, "Insira um valor para o campo \"CPF\"");
                    return;
                }

                // Recupera e valida o telefone do cliente
                string telefone = txbTelefone.Text.Trim();

                // Recupera e valida o email do cliente
                string email = txbEmail.Text.Trim();
                if (string.IsNullOrEmpty(email))
                {
                    Ferramentas.mostraErroTextBox(txbEmail, "Insira um valor para o campo \"Email\"");
                    return;
                }

                // Recupera e valida o endereço
                string rua = txbRua.Text.Trim();
                if (string.IsNullOrEmpty(rua))
                {
                    Ferramentas.mostraErroTextBox(txbRua, "Insira um valor para o campo \"Rua\"");
                    return;
                }

                int numero;
                if (!int.TryParse(txbNumero.Text.Trim(), out numero))
                {
                    Ferramentas.mostraErroTextBox(txbNumero, "Insira um valor válido para o campo \"Número\"");
                    return;
                }

                string bairro = txbBairro.Text.Trim();
                if (string.IsNullOrEmpty(bairro))
                {
                    Ferramentas.mostraErroTextBox(txbBairro, "Insira um valor para o campo \"Bairro\"");
                    return;
                }

                string cidade = txbCidade.Text.Trim();
                if (string.IsNullOrEmpty(cidade))
                {
                    Ferramentas.mostraErroTextBox(txbCidade, "Insira um valor para o campo \"Cidade\"");
                    return;
                }

                string cep = txbCEP.Text.Trim();
                if (string.IsNullOrEmpty(cep))
                {
                    Ferramentas.mostraErroTextBox(txbCEP, "Insira um valor para o campo \"CEP\"");
                    return;
                }

                string complemento = txbComplemento.Text.Trim();

                // Cria um objeto cliente com os dados atualizados
                Cliente cliente = new Cliente(nome, cpf, new Endereco(rua, numero, bairro, cidade, cep, complemento), telefone, email);

                // Cria uma instância de CRUDCliente
                CRUDCliente crudCliente = new CRUDCliente();

                // Atualiza os dados do cliente no banco de dados
                bool resultado = crudCliente.atualizarCliente(cpfClienteOriginal, cliente);
                if (resultado)
                {
                    // Informa ao usuário que os dados foram atualizados com sucesso
                    MessageBox.Show("Dados do cliente atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Limpa os TextBoxes com os dados do cliente
                    clearTextBox();

                    // Exibe uma mensagem de erro informando que a atualização falhou
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Não foi possível atualizar os dados do cliente.");
                    sb.AppendLine("Verifique se existe um cliente cadastrado com o CPF pesquisado!");
                    Ferramentas.mostraErroTextBox(txbCPF, sb.ToString());
                }
            }
            catch (MySqlException erro)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Erro de MySQL: " + erro.Message);
                sb.AppendLine("Código de Erro: " + erro.Number);
                sb.AppendLine("Estado SQL: " + erro.SqlState);
                sb.AppendLine("Stack Trace: " + erro.StackTrace);
                MessageBox.Show(sb.ToString(), "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception erro)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Erro: " + erro.Message);
                sb.AppendLine("Stack Trace: " + erro.StackTrace);
                MessageBox.Show(sb.ToString(), "Erro Desconhecido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                // Recupera o CPF usado na pesquisa e remove os espaços em branco do começo e fim
                string cpf = txbCPF.Text.Trim();
                if (string.IsNullOrEmpty(cpf))
                {
                    Ferramentas.mostraErroTextBox(txbCPF, "Insira um valor para o campo \"CPF\"");
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
                    Ferramentas.mostraErroTextBox(txbCPF, "Não existe um cliente com esse CPF!");
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
    }
}
