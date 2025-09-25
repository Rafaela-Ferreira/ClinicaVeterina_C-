using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetodeExtensao
{
    public partial class FrmDiarias : Form
    {
        private CRUDChacara crudChacara;

        public FrmDiarias()
        {
            InitializeComponent();
            crudChacara = new CRUDChacara();
            CarregarDadosCalendario();
            DestacaCalendario();
            CarregarNomesChacaras();

        }

        private void FrmDiarias_Load(object sender, EventArgs e)
        {
            CRUDChacara crudChacara = new CRUDChacara();
            DataTable tabelaUnificada = crudChacara.CarregarReservasEDatasBloqueadas();
            dataGridView1.DataSource = tabelaUnificada;
        }

        private void btnInicio3_Click(object sender, EventArgs e)
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

        private void btnChacaras3_Click(object sender, EventArgs e)
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

        private void btnClientes3_Click(object sender, EventArgs e)
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

        private void btnVisitas3_Click(object sender, EventArgs e)
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

        private void btnPagamento3_Click(object sender, EventArgs e)
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

        private void btnContratos3_Click(object sender, EventArgs e)
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

        private void btnSair3_Click(object sender, EventArgs e)
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

        private void CarregarDadosCalendario()
        {
            try
            {
                // Instanciar a classe CRUDChacara
                CRUDChacara crudChacara = new CRUDChacara();

                // Obter a tabela combinada de reservas e datas bloqueadas
                DataTable tabelaUnificada = crudChacara.CarregarReservasEDatasBloqueadas();

                // Exibir os dados no DataGridView
                dataGridView1.DataSource = tabelaUnificada;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os dados do calendário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DestacaCalendario()
        {
            // Chama o método da classe CRUDChacara e passa o controle MonthCalendar
            crudChacara.DestacaCalendario(monthCalendar1);
        }

        private void CarregarNomesChacaras()
        {
            try
            {
                // Inicializa o CRUDChacara para obter os nomes das chácaras
                CRUDChacara crud = new CRUDChacara();
                List<string> nomesChacaras = crud.ObterNomesChacaras();

                // Verifica se a lista contém nomes de chácaras
                if (nomesChacaras != null && nomesChacaras.Any())
                {
                    // Limpa o ComboBox para evitar itens duplicados
                    txbChacaraDiaria.Items.Clear();

                    // Adiciona os nomes das chácaras ao ComboBox
                    txbChacaraDiaria.Items.AddRange(nomesChacaras.ToArray());
                }
                else
                {
                    // Exibe uma mensagem se nenhuma chácara for encontrada
                    MessageBox.Show("Nenhuma chácara foi encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (MySqlException sqlEx)
            {
                // Tratamento de erros específicos do MySQL
                MessageBox.Show($"Erro ao acessar o banco de dados: {sqlEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Tratamento de erros gerais
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private int ObterIdChacaraSelecionada()
        {
            // Obtém o nome da chácara selecionada no ComboBox
            string nomeChacaraSelecionada = txbChacaraDiaria.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(nomeChacaraSelecionada))
            {
                return -1; // Retorna -1 se nenhuma chácara estiver selecionada
            }

            CRUDChacara crudChacara = new CRUDChacara();
            return crudChacara.ObterIdChacaraPorNome(nomeChacaraSelecionada);
        }

        private void btnAdicionar2_Click(object sender, EventArgs e)
        {
            try
            {

                // Instanciar a classe CRUDChacaras
                CRUDChacara crudChacara = new CRUDChacara();

                // Obter e validar os valores dos campos
                string cpfCliente = txbCPFDiaria.Text.Trim();
                if (string.IsNullOrWhiteSpace(cpfCliente) || cpfCliente.Length != 11)
                {
                    MessageBox.Show("CPF inválido. Verifique o formato.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validação da data de check-in
                if (!DateTime.TryParseExact(txbCheckin.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataCheckin))
                {
                    MessageBox.Show("Data de check-in inválida. O formato deve ser: dd/MM/yyyy", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validação da data de check-out
                if (!DateTime.TryParseExact(txbCheckout.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataCheckout))
                {
                    MessageBox.Show("Data de check-out inválida. O formato deve ser: dd/MM/yyyy", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string observacoes = txbObs.Text.Trim();
                int idChacara = ObterIdChacaraSelecionada(); // Obtém o ID da chácara selecionada no ComboBox
                                                             
                // Validar inputs
                bool inputsValidados = crudChacara.ValidarInputs(cpfCliente, txbChacaraDiaria, dataCheckin, dataCheckout);

                if (!inputsValidados)
                {
                    return; // Se a validação falhar, não prossegue com o agendamento
                }

                if (idChacara == -1)
                {
                    MessageBox.Show("Erro ao identificar a chácara selecionada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                // Chamar o método para agendar a reserva
                bool sucesso = crudChacara.AgendarReserva(cpfCliente, dataCheckin, dataCheckout, observacoes, idChacara);

                if (sucesso)
                {
                    MessageBox.Show("Reserva confirmada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarDadosCalendario(); // Atualiza o calendário e o DataGridView
                }
                else
                {
                    MessageBox.Show("Erro ao confirmar a reserva. Verifique os dados e tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao agendar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string nomeChacara = dataGridView1.SelectedRows[0].Cells["nomeChacara"].Value.ToString();
                    DateTime dataInicio = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["dataInicio"].Value);
                    DateTime dataFim = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["dataFim"].Value);

                    CRUDChacara crud = new CRUDChacara();
                    bool sucesso = crud.ExcluirReserva(nomeChacara, dataInicio, dataFim);


                    if (sucesso)
                    {
                        MessageBox.Show("Reserva excluída com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarNomesChacaras(); // Atualiza o DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Erro ao excluir a reserva.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Selecione uma reserva para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtualizar2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string nomeChacara = dataGridView1.SelectedRows[0].Cells["nomeChacara"].Value.ToString();
                    DateTime dataInicio = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["dataInicio"].Value);
                    DateTime dataFim = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["dataFim"].Value);
                    string novasObservacoes = txbObs.Text; // Exemplo de campo a ser atualizado

                    CRUDChacara crud = new CRUDChacara();
                    bool sucesso = crud.AtualizarReserva(nomeChacara, dataInicio, dataFim, novasObservacoes);

                    if (sucesso)
                    {
                        MessageBox.Show("Reserva atualizada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarNomesChacaras(); // Atualiza o DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Erro ao atualizar a reserva.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Selecione uma reserva para atualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh2_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarNomesChacaras();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao recarregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
