using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; //usado para abrir novo form
using System.Configuration; //iniciou depois de criar o método automaticamente
using MySql.Data.MySqlClient;

namespace ProjetodeExtensao
{
    public partial class FrmLogin : Form
    {
        Thread nt; //criando variável do tipo Thread

        public FrmLogin()
        {
            InitializeComponent();
        }

        //abrindo conexão com o bando de dados
        private const string connectionString = "Server=localhost;Database=reservas;User ID=root;Password=N@th$Fp197;";


        private void BtnAcessar_Click(object sender, EventArgs e)
        {
            // usada para pegar o texto inserido nos campos de usuário e senha no form
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            // verificação pra ver se os campos estão preenchidos
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            // cria uma conexão com o banco de dados
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    // abre a conexão com o banco de dados
                    conexao.Open();

                    // consulta SQL para verificar se o usuário e a senha são válidos
                    string query = "SELECT COUNT(*) FROM usuario WHERE usuario = @usuario AND senha = @senha";
                    MySqlCommand cmd = new MySqlCommand(query, conexao);

                    // adiciona os valores dos paramêtros para consulta
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    // faz a consulta SQL e converte o resultado para um inteiro
                    int resultado = Convert.ToInt32(cmd.ExecuteScalar());

                    if (resultado > 0) // verifica se o usuário correspondente foi encontrado
                    {
                        this.Close(); // Fecha o formulário de Login
                        nt = new Thread(novoForm); // Inicia a Thread para abrir o novo form
                        nt.SetApartmentState(ApartmentState.STA); // Define o estado da Thread
                        nt.Start(); // Inicia o comando para abrir o novo form
                    }
                    else
                    {
                        // falha na autenticação
                        MessageBox.Show("Usuário ou senha incorretos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar com o banco de dados: " + ex.Message);
                }
            }

        }

        private void novoForm() //método criado para rodar a aplicação
        {
            Application.Run(new FrmInicial()); //new usado para instanciar a próxima tela (FrmInicial)
        }
    }
}
