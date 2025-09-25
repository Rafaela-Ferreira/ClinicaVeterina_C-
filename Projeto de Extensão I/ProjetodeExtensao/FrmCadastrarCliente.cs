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
    public partial class FrmCadastrarCliente : Form
    {
        public FrmCadastrarCliente()
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

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Recupera e valida os dados dos TextBoxes
            string nome = txbNome.Text.Trim();
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Insira um valor para o campo \"Nome\"");
                return;
            }

            string cpf = txbCPF.Text.Trim();
            if (string.IsNullOrEmpty(cpf))
            {
                MessageBox.Show("Insira um valor para o campo \"CPF\"");
                return;
            }

            string sexo = txbSexo.Text.Trim();
            if (string.IsNullOrEmpty(sexo))
            {
                MessageBox.Show("Insira um valor para o campo \"Sexo\"");
                return;
            }

            string telefone = txbTelefone.Text.Trim();
            if (string.IsNullOrEmpty(telefone))
            {
                MessageBox.Show("Insira um valor para o campo \"Telefone\"");
                return;
            }

            string email = txbEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Insira um valor para o campo \"Email\"");
                return;
            }

            // Recupera e valida os dados do endereço
            string rua = txbRua.Text.Trim();
            if (string.IsNullOrEmpty(rua))
            {
                MessageBox.Show("Insira um valor para o campo \"Rua\"");
                return;
            }

            int numero;
            try
            {
                numero = int.Parse(txbNumero.Text.Trim());
            }
            catch (FormatException)
            {
                MessageBox.Show("Insira um valor válido para o campo \"Número\"");
                return;
            }

            string bairro = txbBairro.Text.Trim();
            if (string.IsNullOrEmpty(bairro))
            {
                MessageBox.Show("Insira um valor para o campo \"Bairro\"");
                return;
            }

            string cidade = txbCidade.Text.Trim();
            if (string.IsNullOrEmpty(cidade))
            {
                MessageBox.Show("Insira um valor para o campo \"Cidade\"");
                return;
            }

            string cep = txbCEP.Text.Trim();
            if (string.IsNullOrEmpty(cep))
            {
                MessageBox.Show("Insira um valor para o campo \"CEP\"");
                return;
            }

            string complemento = txbComplemento.Text.Trim();

            try
            {
                // Cria um objeto Endereco com os dados dos TextBoxes
                Endereco endereco = new Endereco(rua, numero, bairro, cidade, cep, complemento);

                // Cria um objeto Cliente com os dados dos TextBoxes e o objeto Endereco
                Cliente cliente = new Cliente(nome, cpf, endereco, email, sexo);

                // Cria um objeto Telefone com os dados do TextBox de telefone e o objeto Cliente
                Telefone telefoneObj = new Telefone(telefone, cliente);

                // Cria uma instância do CRUDCliente
                CRUDCliente CRUDcliente = new CRUDCliente();

                // O idUsuario é sempre 1
                int idUsuario = 1;

                // Cadastra o cliente e o telefone no banco
                CRUDcliente.inserirCliente(cliente, telefoneObj, idUsuario);

                // Limpa os TextBoxes após o cadastro
                clearTextBox();

                // Informa o usuário que o cliente foi cadastrado no banco
                MessageBox.Show("Dados Salvos no Banco!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException erro)
            {
                MessageBox.Show($"Erro no banco de dados: {erro.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception erro)
            {
                MessageBox.Show($"Erro desconhecido: {erro.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearTextBox()
        {
            txbNome.Clear();
            txbCPF.Clear();
            txbSexo.Clear();
            txbTelefone.Clear();
            txbEmail.Clear();
            txbRua.Clear();
            txbNumero.Clear();
            txbBairro.Clear();
            txbCidade.Clear();
            txbCEP.Clear();
            txbComplemento.Clear();
        }


    }
}
