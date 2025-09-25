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
    public partial class FrmExcluirChacara : Form
    {

        private CRUDChacara CRUDchacara;
        public FrmExcluirChacara()
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

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                // Recupera o nome da chácara usado na pesquisa e remove os espaços em branco do começo e fim
                string nomeChacara = txbPesquisa.Text.Trim();
                if (string.IsNullOrEmpty(nomeChacara))
                {
                    Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"Pesquisar Nome da Chácara\"");
                    return;
                }

                // Pesquisa se existe uma chácara com o nome fornecido
                Chacara chacara = CRUDchacara.pesquisarChacaraPorNome(nomeChacara);

                // Caso encontre uma chácara, a referência será diferente de null
                if (chacara != null)
                {
                    // Preenche os campos com os dados da chácara encontrada
                    txbNomeChacara.Text = chacara.GetNomeChacara();
                    txbDescricao.Text = chacara.GetDescricao();
                    // Preencha outros campos conforme necessário
                    Endereco endereco = chacara.GetEndereco();

                    // Preenche cada TextBox com as informações do endereço
                    txbRua.Text = endereco.GetRua();
                    txbNumero.Text = endereco.GetNumero().ToString();
                    txbBairro.Text = endereco.GetBairro();
                    txbCidade.Text = endereco.GetCidade();
                    txbCEP.Text = endereco.GetCEP();
                    txbComplemento.Text = endereco.GetComplemento();
                }
                // chacara == null -> não encontrou uma chácara com o nome passado
                else
                {
                    Ferramentas.mostraErroTextBox(txbPesquisa, "Não existe uma chácara com esse nome!");
                }
            }
            // Trata erros relacionados ao banco de dados
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
            // Trata outros erros que possam ocorrer
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

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Recupera o nome da chácara usado na pesquisa e remove os espaços em branco do começo e fim
            string nomeChacara = txbPesquisa.Text.Trim();
            if (nomeChacara == "")
            {
                Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"Nome da Chácara - Pesquisar\"");
                return;
            }

            // Instancia o objeto de CRUD para Chácara
            CRUDChacara crudChacara = new CRUDChacara();

            // Caso encontre uma chácara com o nome digitado e consiga removê-la
            if (crudChacara.removerChacaraPorNome(nomeChacara) == true)
            {
                // Limpa o TextBox de pesquisa
                txbNomeChacara.Clear();

                // Limpa os TextBoxes com os dados da chácara
                clearTextBox();

                // Informa ao usuário que a chácara foi removida com sucesso
                MessageBox.Show("Chácara removida do cadastro com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Caso não encontre uma chácara com o nome digitado ou ocorra um erro na remoção
            else
            {
                // Limpa os TextBoxes com os dados da chácara
                clearTextBox();

                // Exibe uma mensagem de erro informando que a remoção falhou
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Não foi possível remover a chácara do cadastro!");
                sb.AppendLine("Verifique se existe uma chácara cadastrada com o nome pesquisado!");
                Ferramentas.mostraErroTextBox(txbNomeChacara, sb.ToString());
            }

            //limpa os texbox de dados
            clearTextBox();
        }
    }
}
