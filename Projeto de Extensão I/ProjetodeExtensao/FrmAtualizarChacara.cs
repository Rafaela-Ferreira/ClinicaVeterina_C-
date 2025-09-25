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
    public partial class FrmAtualizarChacara : Form
    {

        private CRUDChacara CRUDchacara;

        public FrmAtualizarChacara()
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
            // Recupera o nome da chácara usado na pesquisa
            string nomeChacara = txbPesquisa.Text.Trim();
            if (nomeChacara == "")
            {
                Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"Nome da Chácara - Pesquisar\"");
                return;
            }

            // Pesquisa se existe uma chácara com o nome passado
            Chacara chacara = CRUDchacara.pesquisarChacaraPorNome(nomeChacara);

            // Caso encontre uma chácara, a referência será diferente de null
            if (chacara != null)
            {
                txbNomeChacara.Text = chacara.GetNomeChacara();
                txbDescricao.Text = chacara.GetDescricao();
                txbRua.Text = chacara.GetEndereco().GetRua();
                txbNumero.Text = chacara.GetEndereco().GetNumero().ToString();
                txbBairro.Text = chacara.GetEndereco().GetBairro();
                txbCidade.Text = chacara.GetEndereco().GetCidade();
                txbCEP.Text = chacara.GetEndereco().GetCEP();
                txbComplemento.Text = chacara.GetEndereco().GetComplemento() ?? "";
            }
            // chacara == null -> não encontrou uma chácara com o nome passado
            else
            {
                // Limpa os textbox com os dados da chácara
                clearTextBox();
                // Mostra uma mensagem de erro
                Ferramentas.mostraErroTextBox(txbNomeChacara, "Não existe uma chácara com esse nome!");
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

        private void btnAdicionar0_Click(object sender, EventArgs e)
        {
                try
                {
                    // Recupera o nome da chácara usado na pesquisa e remove os espaços em branco do começo e fim
                    string nomeChacara = txbPesquisa.Text.Trim();
                    if (nomeChacara == "")
                    {
                        Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"Nome da Chácara - Pesquisar\"");
                        return;
                    }

                    // Recupera o texto do componente textbox e remove os espaços em branco do começo e fim
                    string novoNome = txbNomeChacara.Text.Trim();
                    if (novoNome == "")
                    {
                        Ferramentas.mostraErroTextBox(txbPesquisa, "Insira um valor para o campo \"Nome da Chácara\"");
                        return;
                    }

                    // Recupera e valida os dados dos componentes textbox
                    string descricao = txbDescricao.Text.Trim();
                    if (descricao == "")
                    {
                        Ferramentas.mostraErroTextBox(txbDescricao, "Insira uma descrição para a chácara");
                        return;
                    }

                    string rua = txbRua.Text.Trim();
                    if (rua == "")
                    {
                        Ferramentas.mostraErroTextBox(txbRua, "Insira um valor para o campo \"Rua\"");
                        return;
                    }

                    int numero;
                    try
                    {
                        numero = int.Parse(txbNumero.Text.Trim());
                    }
                    catch (FormatException)
                    {
                        Ferramentas.mostraErroTextBox(txbNumero, "Insira um valor válido para o campo \"Número\"");
                        return;
                    }

                    string bairro = txbBairro.Text.Trim();
                    if (bairro == "")
                    {
                        Ferramentas.mostraErroTextBox(txbBairro, "Insira um valor para o campo \"Bairro\"");
                        return;
                    }

                    string cidade = txbCidade.Text.Trim();
                    if (cidade == "")
                    {
                        Ferramentas.mostraErroTextBox(txbCidade, "Insira um valor para o campo \"Cidade\"");
                        return;
                    }

                    string cep = txbCEP.Text.Trim();
                    if (cep == "")
                    {
                        Ferramentas.mostraErroTextBox(txbCEP, "Insira um valor para o campo \"CEP\"");
                        return;
                    }

                    string complemento = txbComplemento.Text.Trim();

                    // Cria um objeto chácara com os dados atualizados
                    Chacara chacara = new Chacara(novoNome, descricao, new Endereco(rua, numero, bairro, cidade, cep, complemento));

                    // Atualiza os dados da chácara que possui o nome pesquisado
                    bool rs = CRUDchacara.atualizarChacara(nomeChacara, chacara);
                    if (rs)
                    {
                        // Informa ao usuário que os dados foram atualizados com sucesso
                        MessageBox.Show("Dados atualizados no banco!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Limpa os textbox com os dados da chácara
                        clearTextBox();

                        // Exibe uma mensagem de erro informando que não encontrou uma chácara com o nome pesquisado
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Não foi possível atualizar os dados");
                        sb.AppendLine("Verifique se existe uma chácara cadastrada com o nome pesquisado!");
                        Ferramentas.mostraErroTextBox(txbNomeChacara, sb.ToString());
                    }
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
                // Tratamento dos demais erros que possam ocorrer
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
                      

          

    }
}
