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
    public partial class FrmPagamento : Form
    {
        private CRUDChacara CRUDchacara; //add now
        private DataTable dataTable; //add now

        public FrmPagamento()
        {
            InitializeComponent();

            this.CRUDchacara = new CRUDChacara(); //add now

            gvPagamento.CellEndEdit += new DataGridViewCellEventHandler(gvPagamento_CellEndEdit);
        }

        private void btnInicio5_Click(object sender, EventArgs e)
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

        private void btnChacaras5_Click(object sender, EventArgs e)
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

        private void btnClientes5_Click(object sender, EventArgs e)
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

        private void btnDiarias5_Click(object sender, EventArgs e)
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

        private void btnVisitas5_Click(object sender, EventArgs e)
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

        private void btnContratos5_Click(object sender, EventArgs e)
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

        private void btnSair5_Click(object sender, EventArgs e)
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

        private void FrmPagamento_Load(object sender, EventArgs e)
        {
            //add now

            // Cria uma tabela vazia
            DataTable dataTable3 = new DataTable();

            // Instancia o objeto CRUD para trabalhar com as chácaras
            CRUDChacara mostrarCliente = new CRUDChacara();

            // Insere os dados do relatório na tabela
            mostrarCliente.mostrarPagamento(dataTable3);

            // Define a fonte de dados do DataGridView para a tabela gerada
            gvPagamento.DataSource = dataTable3;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataTable dataTable3 = new DataTable();

            // Preencha a tabela com dados do banco de dados
            string connectionStr = "Server=localhost;Database=reservas;User ID=root;Password=N@th$Fp197;";

            using (MySqlConnection connection = new MySqlConnection(connectionStr))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM pagamento";

                    
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable3);
                    }

                    gvPagamento.DataSource = dataTable3;

                    MessageBox.Show("Dados carregados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Atualiza a fonte de dados do DataGridView
            gvPagamento.DataSource = dataTable3;
        }

        private void gvPagamento_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a célula editada é válida
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtém o ID do pagamento da linha editada
                int idPagamento = Convert.ToInt32(gvPagamento.Rows[e.RowIndex].Cells["idpagamento"].Value);
                string columnName = gvPagamento.Columns[e.ColumnIndex].Name;
                object newValue = gvPagamento.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                // Atualiza o banco de dados com o novo valor
                AtualizarPagamento(idPagamento, columnName, newValue);
            }
        }


        private void AtualizarPagamento(int idPagamento, string columnName, object newValue)
        {
            // Certifique-se de usar a conexão e os detalhes corretos do banco de dados
            string connectionStr = "Server=localhost;Database=reservas;User ID=root;Password=N@th$Fp197;";

            using (MySqlConnection connection = new MySqlConnection(connectionStr))
            {
                try
                {
                    connection.Open();

                    // Verifique se o nome da coluna é válido e corresponde a uma coluna na tabela `pagamento`
                    // Aqui assumimos que você já validou que columnName é 'statusPagamento' ou outros nomes válidos

                    // Corrija a consulta SQL para usar o nome correto da coluna
                    string query = $"UPDATE pagamento SET {columnName} = @newValue WHERE idpagamento = @idpagamento";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@newValue", newValue ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@idpagamento", idPagamento);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Dados atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Nenhum dado foi atualizado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erro ao atualizar pagamento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
