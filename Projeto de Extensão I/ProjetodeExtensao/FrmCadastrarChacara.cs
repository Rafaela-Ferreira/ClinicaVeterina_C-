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
    public partial class FrmCadastrarChacara : Form
    {

        private CRUDChacara CRUDchacara;
        public FrmCadastrarChacara()
        {
            InitializeComponent();
            this.CRUDchacara = new CRUDChacara();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
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

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Recupera os textos dos componentes TextBox e remove os espaços em branco
            string nomeChacara = txbNomeChacara.Text.Trim();
            if (string.IsNullOrEmpty(nomeChacara))
            {
                MessageBox.Show("Insira um valor para o campo \"Nome da Chácara\"");
                return;
            }

            string descricao = txbDescricao.Text.Trim();

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

                // Cria um objeto Chacara com os dados dos TextBoxes e o objeto Endereco
                Chacara chacara = new Chacara(nomeChacara, descricao, endereco);

                // Cria uma instância do cadastro
                CRUDChacara CRUDchacara = new CRUDChacara();

                // Cadastra a chácara no banco
                CRUDchacara.inserirChacara(chacara);

                // Limpa os TextBoxes após o cadastro
                clearTextBox();

                // Informa o usuário que a chácara foi cadastrada no banco
                MessageBox.Show("Dados Salvos no Banco!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Trata os erros relacionados ao banco
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
            // Trata os demais erros que possam ocorrer
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
            txbNomeChacara.Clear();
            txbDescricao.Clear();
            txbRua.Clear();
            txbNumero.Clear();
            txbBairro.Clear();
            txbCidade.Clear();
            txbCEP.Clear();
            txbComplemento.Clear();
        }

    }
}
