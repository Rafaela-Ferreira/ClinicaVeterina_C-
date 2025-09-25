using MySql.Data.MySqlClient;
using ProjetodeExtensao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace ProjetodeExtensao
{
    public class CRUDChacara
    {
        private string connectionStr;

        //-------------------------------------------------------------------------

        public CRUDChacara()
        {
            connectionStr = "Server=localhost;Database=reservas;User ID=root;Password=N@th$Fp197;";
        }

        //-------------------------------------------------------------------------

        public void inserirChacara(Chacara chacara)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdInsert = null;

            try
            {
                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdInsert = new MySqlCommand();
                cmdInsert.Connection = connectionBD;

                // Inserir o endereço primeiro
                cmdInsert.CommandText = "INSERT INTO endereco (rua, numero, bairro, cidade, cep, complemento) " +
                                        "VALUES (@rua, @numero, @bairro, @cidade, @cep, @complemento); " +
                                        "SET @idEndereco = LAST_INSERT_ID();";

                cmdInsert.Parameters.AddWithValue("rua", chacara.GetEndereco().GetRua());
                cmdInsert.Parameters.AddWithValue("numero", chacara.GetEndereco().GetNumero());
                cmdInsert.Parameters.AddWithValue("bairro", chacara.GetEndereco().GetBairro());
                cmdInsert.Parameters.AddWithValue("cidade", chacara.GetEndereco().GetCidade());
                cmdInsert.Parameters.AddWithValue("cep", chacara.GetEndereco().GetCEP());
                cmdInsert.Parameters.AddWithValue("complemento", chacara.GetEndereco().GetComplemento());

                cmdInsert.Parameters.Add("@idEndereco", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmdInsert.ExecuteNonQuery();

                int idEndereco = Convert.ToInt32(cmdInsert.Parameters["@idEndereco"].Value);

                // Inserir a chácara
                cmdInsert.CommandText = "INSERT INTO chacara (nomeChacara, idEndereco, descricao) " +
                                        "VALUES (@nomeChacara, @idEndereco, @descricao)";
                cmdInsert.Parameters.Clear();
                cmdInsert.Parameters.AddWithValue("nomeChacara", chacara.GetNomeChacara());
                cmdInsert.Parameters.AddWithValue("idEndereco", idEndereco);
                cmdInsert.Parameters.AddWithValue("descricao", chacara.GetDescricao());

                cmdInsert.ExecuteNonQuery();
            }
            finally
            {
                if (cmdInsert != null) cmdInsert.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }
        }

        //-------------------------------------------------------------------------

        public Chacara pesquisarChacaraPorNome(string nomeChacara)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdSelect = null;
            Chacara chacara = null;

            try
            {
                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdSelect = new MySqlCommand();
                cmdSelect.Connection = connectionBD;

                cmdSelect.CommandText = "SELECT c.nomeChacara, c.descricao, e.rua, e.numero, e.bairro, e.cidade, e.cep, e.complemento " +
                                        "FROM chacara c " +
                                        "INNER JOIN endereco e ON c.idEndereco = e.idendereco " +
                                        "WHERE c.nomeChacara = @nomeChacara";

                cmdSelect.Parameters.AddWithValue("nomeChacara", nomeChacara);

                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    string nome = reader.GetString(0);  // nomeChacara
                    string descricao = reader.GetString(1);  // descricao
                    Endereco endereco = new Endereco(
                        reader.GetString(2),  // rua
                        reader.GetInt32(3),   // numero
                        reader.GetString(4),  // bairro
                        reader.GetString(5),  // cidade
                        reader.GetString(6),  // cep
                        reader.GetString(7)   // complemento
                    );

                    chacara = new Chacara(nome, descricao, endereco);
               
                }

                reader.Close();
            }
            finally
            {
                if (cmdSelect != null) cmdSelect.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }

            return chacara;
        }

        //-------------------------------------------------------------------------


        public bool removerChacaraPorNome(string nomeChacara)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdDelete = null;

            try
            {
                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdDelete = new MySqlCommand();
                cmdDelete.Connection = connectionBD;

                // Remover a chácara
                cmdDelete.CommandText = "DELETE FROM chacara WHERE nomeChacara = @nomeChacara";
                cmdDelete.Parameters.Clear();
                cmdDelete.Parameters.AddWithValue("@nomeChacara", nomeChacara);
                int rowsAffectedChacara = cmdDelete.ExecuteNonQuery();

                // Verificar se a exclusão da chácara foi bem-sucedida
                if (rowsAffectedChacara > 0)
                {
                    Console.WriteLine("Chácara removida com sucesso: " + nomeChacara);

                    // Obter o endereço associado antes de remover
                    Chacara chacara = pesquisarChacaraPorNome(nomeChacara);
                    if (chacara != null)
                    {
                        // Remover o endereço
                        cmdDelete.CommandText = "DELETE FROM endereco WHERE idendereco = @idEndereco";
                        cmdDelete.Parameters.Clear();
                        cmdDelete.Parameters.AddWithValue("@idEndereco", chacara.GetEndereco());
                        cmdDelete.ExecuteNonQuery();
                        Console.WriteLine("Endereço removido com sucesso para a chácara: " + nomeChacara);
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("Nenhuma chácara encontrada com o nome: " + nomeChacara);
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erro de MySQL: " + ex.Message);
                return false;
            }
            finally
            {
                if (cmdDelete != null) cmdDelete.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }
        }




        //-------------------------------------------------------------------------

        public bool atualizarChacara(string nomeChacara, Chacara novaChacara)
        {
            int numLinhasAfetadas = 0;
            MySqlConnection connectionBD = null;
            MySqlCommand cmdUpdate = null;

            try
            {
                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdUpdate = new MySqlCommand();
                cmdUpdate.Connection = connectionBD;

                // Atualizar o endereço
                cmdUpdate.CommandText = "UPDATE endereco SET " +
                                        "rua = @rua, " +
                                        "numero = @numero, " +
                                        "bairro = @bairro, " +
                                        "cidade = @cidade, " +
                                        "cep = @cep, " +
                                        "complemento = @complemento "; //+
                                        //"WHERE idendereco = @idEndereco";

                cmdUpdate.Parameters.AddWithValue("rua", novaChacara.GetEndereco().GetRua());
                cmdUpdate.Parameters.AddWithValue("numero", novaChacara.GetEndereco().GetNumero());
                cmdUpdate.Parameters.AddWithValue("bairro", novaChacara.GetEndereco().GetBairro());
                cmdUpdate.Parameters.AddWithValue("cidade", novaChacara.GetEndereco().GetCidade());
                cmdUpdate.Parameters.AddWithValue("cep", novaChacara.GetEndereco().GetCEP());
                cmdUpdate.Parameters.AddWithValue("complemento", novaChacara.GetEndereco().GetComplemento());
                //cmdUpdate.Parameters.AddWithValue("idEndereco", novaChacara.GetEndereco());

                numLinhasAfetadas = cmdUpdate.ExecuteNonQuery();

                // Atualizar a chácara
                cmdUpdate.CommandText = "UPDATE chacara SET " +
                                        "nomeChacara = @nomeChacara, " +
                                        "descricao = @descricao " +
                                        "WHERE nomeChacara = @nomeChacaraOriginal";

                cmdUpdate.Parameters.Clear();
                cmdUpdate.Parameters.AddWithValue("nomeChacara", novaChacara.GetNomeChacara());
                cmdUpdate.Parameters.AddWithValue("descricao", novaChacara.GetDescricao());
                cmdUpdate.Parameters.AddWithValue("nomeChacaraOriginal", nomeChacara);

                numLinhasAfetadas += cmdUpdate.ExecuteNonQuery();
            }
            finally
            {
                if (cmdUpdate != null) cmdUpdate.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }

            return numLinhasAfetadas > 0;
        }

        //-------------------------------------------------------------------------

        public void gerarRelatorioChacaras(DataTable table)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdSelect = null;

            try
            {
                table.Clear();

                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdSelect = new MySqlCommand();
                cmdSelect.Connection = connectionBD;

                cmdSelect.CommandText = "SELECT c.nomeChacara AS 'Chácara', e.rua AS Rua, e.numero as 'Número', e.bairro AS Bairro, e.cidade AS Cidade, e.cep AS CEP, e.complemento AS Complemento, c.descricao AS 'Descrição'" +
                                        "FROM chacara c " +
                                        "INNER JOIN endereco e ON c.idEndereco = e.idendereco";

                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.HasRows)
                {
                    table.Load(reader);
                }

                reader.Close();
            }
            finally
            {
                if (cmdSelect != null) cmdSelect.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }
        }

        //-------------------------------------------------------------------------

        //add now
        public void mostrarClientes(DataTable table2)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdSelect = null;

            try
            {
                table2.Clear();

                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdSelect = new MySqlCommand();
                cmdSelect.Connection = connectionBD;

                cmdSelect.CommandText = "SELECT \r\n    nome AS Cliente,\r\n    cpf AS CPF,\r\n    email AS Email,\r\n    sexo AS Sexo\r\nFROM \r\n    cliente;\r\n";

                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.HasRows)
                {
                    table2.Load(reader);
                }

                reader.Close();
            }
            finally
            {
                if (cmdSelect != null) cmdSelect.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }
        }

        //add now
        public void mostrarPagamento(DataTable table2)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdSelect = null;

            try
            {
                table2.Clear();

                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdSelect = new MySqlCommand();
                cmdSelect.Connection = connectionBD;

                //cmdSelect.CommandText = "SELECT \r\n    c.nome AS Cliente,\r\n    ch.nomeChacara AS 'Chácara',\r\n    p.statusPagamento AS `Status do Pagamento`\r\nFROM \r\n    pagamento p\r\nJOIN \r\n    cliente c ON p.idCliente = c.idcliente\r\nJOIN \r\n    reserva r ON p.idReserva = r.idreserva\r\nJOIN \r\n    chacara ch ON r.idChacara = ch.idchacara;\r\n";

                /*cmdSelect.CommandText = "SELECT p.idpagamento, c.nome AS 'Cliente', ch.nomeChacara AS 'Chácara', p.statusPagamento AS `Status do Pagamento` " +
                                "FROM pagamento p " +
                                "JOIN cliente c ON p.idCliente = c.idcliente " +
                                "JOIN reserva r ON p.idReserva = r.idreserva " +
                                "JOIN chacara ch ON r.idChacara = ch.idchacara;";*/

                cmdSelect.CommandText = "SELECT * FROM pagamento";

                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.HasRows)
                {
                    table2.Load(reader);
                }

                reader.Close();
            }
            finally
            {
                if (cmdSelect != null) cmdSelect.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }
        }

        public void mostrarContratos(DataTable table2)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdSelect = null;

            try
            {
                table2.Clear();

                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdSelect = new MySqlCommand();
                cmdSelect.Connection = connectionBD;

                cmdSelect.CommandText = "SELECT \r\n    c.nome AS Cliente,\r\n    c.cpf AS 'CPF Cliente',\r\n    ch.nomeChacara AS Chacara,\r\n    r.dataInicio AS 'Data da Reserva'\r\nFROM \r\n    reserva r\r\nJOIN \r\n    cliente c ON r.idCliente = c.idcliente\r\nJOIN \r\n    chacara ch ON r.idChacara = ch.idchacara;\r\n";

                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.HasRows)
                {
                    table2.Load(reader);
                }

                reader.Close();
            }
            finally
            {
                if (cmdSelect != null) cmdSelect.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }
        }

        //-------------------------------------------------------------------------

        public DataTable CarregarReservasEDatasBloqueadas()
        {
            DataTable tabelaUnificada = new DataTable();

            // Definir as colunas da DataTable
            tabelaUnificada.Columns.Add("Tipo", typeof(string)); // Reserva ou Data Bloqueada
            tabelaUnificada.Columns.Add("Descricao", typeof(string)); // Motivo ou Observações
            tabelaUnificada.Columns.Add("Cliente", typeof(string)); // Nome do Cliente
            tabelaUnificada.Columns.Add("Chacara", typeof(string)); // Nome da Chácara
            tabelaUnificada.Columns.Add("DataInicio", typeof(DateTime));
            tabelaUnificada.Columns.Add("DataFim", typeof(DateTime));

            try
            {
                using (MySqlConnection connectionBD = new MySqlConnection(connectionStr))
                {
                    connectionBD.Open();

                    // Consultas SQL
                    string queryReservas = @"
                SELECT  
                    'Reserva' AS Tipo,
                    r.Observacoes AS Descricao, 
                    c.nome AS Cliente, 
                    ch.nomeChacara AS Chacara,
                    r.dataInicio AS DataInicio, 
                    r.dataFim AS DataFim
                FROM reserva r
                JOIN cliente c ON r.idCliente = c.idCliente
                JOIN chacara ch ON r.idChacara = ch.idChacara
                ORDER BY r.dataInicio";

                    string queryDatasBloqueadas = @"
                SELECT 
                    'Data Bloqueada' AS Tipo,
                    motivo AS Descricao, 
                    NULL AS Cliente,
                    NULL AS Chacara,
                    data AS DataInicio,
                    NULL AS DataFim
                FROM datasBloqueadas
                ORDER BY data";

                    // Carregar dados de reservas
                    using (MySqlCommand cmd = new MySqlCommand(queryReservas, connectionBD))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            AdicionarDadosTabelaUnificada(reader, tabelaUnificada, "Reserva");
                        }
                    }

                    // Carregar dados de datas bloqueadas
                    using (MySqlCommand cmd = new MySqlCommand(queryDatasBloqueadas, connectionBD))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            AdicionarDadosTabelaUnificada(reader, tabelaUnificada, "Data Bloqueada");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Exibir mensagem de erro, se necessário
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return tabelaUnificada;
        }

        //-------------------------------------------------------------------------

        public DataTable CarregarVisitas()
        {
            DataTable tabelaUnificada = new DataTable();

            // Definir as colunas da DataTable
            tabelaUnificada.Columns.Add("Tipo", typeof(string)); // Reserva, Data Bloqueada ou Visita
            tabelaUnificada.Columns.Add("Descricao", typeof(string)); // Motivo, Observações ou Cliente para Visitas
            tabelaUnificada.Columns.Add("Cliente", typeof(string)); // Nome do Cliente
            tabelaUnificada.Columns.Add("Chacara", typeof(string)); // Nome da Chácara
            tabelaUnificada.Columns.Add("Data", typeof(DateTime)); // Data de Início para Reservas e Datas Bloqueadas
            tabelaUnificada.Columns.Add("Hora", typeof(DateTime)); // Hora para Visitas (pode ser NULL para outros tipos)

            try
            {
                using (MySqlConnection connectionBD = new MySqlConnection(connectionStr))
                {
                    connectionBD.Open();

                    // Consultas SQL
                    string queryReservas = @"
                    SELECT  
                    'Reserva' AS Tipo,
                    r.Observacoes AS Descricao, 
                    c.nome AS Cliente, 
                    ch.nomeChacara AS Chacara,
                    r.dataInicio AS DataInicio, 
                    r.dataFim AS DataFim,
                    NULL AS Hora
                FROM reserva r
                JOIN cliente c ON r.idCliente = c.idCliente
                JOIN chacara ch ON r.idChacara = ch.idChacara
                ORDER BY r.dataInicio";

                    string queryDatasBloqueadas = @"
                SELECT 
                    'Data Bloqueada' AS Tipo,
                    motivo AS Descricao, 
                    NULL AS Cliente,
                    NULL AS Chacara,
                    data AS DataInicio,
                    NULL AS DataFim,
                    NULL AS Hora
                FROM datasBloqueadas
                ORDER BY data";

                    string queryVisitas = @"
                SELECT 
                    'Visita' AS Tipo,
                    CONCAT('Visita de ', c.nome) AS Descricao,
                    c.nome AS Cliente, 
                    ch.nomeChacara AS Chacara,
                    v.dataVisita AS DataInicio,
                    NULL AS DataFim,
                    v.horaVisita AS Hora
                FROM visita v
                JOIN cliente c ON v.idCliente = c.idCliente
                JOIN chacara ch ON v.idChacara = ch.idChacara
                ORDER BY v.dataVisita, v.horaVisita";

                    // Carregar dados de reservas
                    using (MySqlCommand cmd = new MySqlCommand(queryReservas, connectionBD))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            AdicionarDadosTabelaVisita(reader, tabelaUnificada, "Reserva");
                        }
                    }

                    // Carregar dados de datas bloqueadas
                    using (MySqlCommand cmd = new MySqlCommand(queryDatasBloqueadas, connectionBD))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            AdicionarDadosTabelaVisita(reader, tabelaUnificada, "Data Bloqueada");
                        }
                    }

                    // Carregar dados de visitas
                    using (MySqlCommand cmd = new MySqlCommand(queryVisitas, connectionBD))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            AdicionarDadosTabelaVisita(reader, tabelaUnificada, "Visita");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Exibir mensagem de erro, se necessário
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return tabelaUnificada;
        }

        // Método auxiliar para adicionar dados na tabela unificada
        private void AdicionarDadosTabelaVisita(MySqlDataReader reader, DataTable tabela, string tipo)
        {
            while (reader.Read())
            {
                DataRow row = tabela.NewRow();
                row["Tipo"] = reader["Tipo"].ToString();
                row["Descricao"] = reader["Descricao"].ToString();
                row["Cliente"] = reader["Cliente"]?.ToString(); // Pode ser NULL para datas bloqueadas
                row["Chacara"] = reader["Chacara"]?.ToString(); // Pode ser NULL para datas bloqueadas
                row["DataInicio"] = reader["DataInicio"] != DBNull.Value ? Convert.ToDateTime(reader["DataInicio"]) : (DateTime?)null;
                row["DataFim"] = reader["DataFim"] != DBNull.Value ? Convert.ToDateTime(reader["DataFim"]) : (DateTime?)null;
                row["Hora"] = reader["Hora"] != DBNull.Value ? Convert.ToDateTime(reader["Hora"]) : (DateTime?)null;
                tabela.Rows.Add(row);
            }
        }

        //-------------------------------------------------------------------------

        // Método para verificar a compatibilidade dos tipos de dados entre DataTables
        public void VerificarCompatibilidadeDataTables(DataTable sourceTable, DataTable targetTable)
        {
            foreach (DataColumn coluna in sourceTable.Columns)
            {
                if (targetTable.Columns.Contains(coluna.ColumnName))
                {
                    // Ajusta o tipo de dado da coluna se for necessário
                    if (targetTable.Columns[coluna.ColumnName].DataType != coluna.DataType)
                    {
                        targetTable.Columns[coluna.ColumnName].DataType = coluna.DataType;
                    }
                }
            }
        }

         //-------------------------------------------------------------------------

        // Método auxiliar para adicionar dados à tabela unificada
        private void AdicionarDadosTabelaUnificada(MySqlDataReader reader, DataTable tabelaUnificada, string tipo)
        {
            while (reader.Read())
            {
                tabelaUnificada.Rows.Add(
                    tipo,
                    reader["Descricao"] ?? DBNull.Value,
                    reader["Cliente"] ?? DBNull.Value,
                    reader["Chacara"] ?? DBNull.Value,
                    reader["DataInicio"] ?? DBNull.Value,
                    reader["DataFim"] ?? DBNull.Value
                );
            }
        }

        //-------------------------------------------------------------------------

        // Exemplo de método para mesclar tabelas
        public DataTable MesclarTabelas(DataTable sourceTable, DataTable targetTable)
        {
            try
            {
                // Verificar a compatibilidade das tabelas
                VerificarCompatibilidadeDataTables(sourceTable, targetTable);

                // Mesclar as tabelas
                targetTable.Merge(sourceTable);
            }
            catch (Exception ex)
            {
                // Tratar o erro adequadamente
                Console.WriteLine("Erro ao mesclar tabelas: " + ex.Message);
            }

            return targetTable;
        }

        //-------------------------------------------------------------------------

        // Método para obter os intervalos reservados
        public List<(DateTime dataInicio, DateTime dataFim)> GetIntervalosReservados()
        {
            var intervalosReservados = new List<(DateTime dataInicio, DateTime dataFim)>();
            string query = "SELECT dataInicio, dataFim FROM reserva";

            using (MySqlConnection connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        intervalosReservados.Add((reader.GetDateTime("dataInicio"), reader.GetDateTime("dataFim")));
                    }
                }
            }

            return intervalosReservados;
        }

        //-------------------------------------------------------------------------

        // Método para obter as datas bloqueadas
        public DateTime[] GetDatasBloqueadas()
        {
            var datasBloqueadas = new List<DateTime>();
            string query = "SELECT data FROM datasBloqueadas"; 

            using (MySqlConnection connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        datasBloqueadas.Add(reader.GetDateTime("data"));
                    }
                }
            }

            return datasBloqueadas.ToArray();
        }

        //-------------------------------------------------------------------------

        public void DestacaCalendario(MonthCalendar calendar)
        {
            try
            {
                // Obter datas bloqueadas e intervalos reservados
                var datasBloqueadas = GetDatasBloqueadas();
                var intervalosReservados = GetIntervalosReservados();

                // Verificar se as datas foram carregadas corretamente
                if (datasBloqueadas == null || intervalosReservados == null)
                {
                    MessageBox.Show("Erro ao carregar as datas. Verifique a conexão com o banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Limpar todas as datas destacadas em negrito anteriormente
                calendar.RemoveAllBoldedDates();

                foreach (DateTime date in datasBloqueadas)
                {
                    calendar.AddBoldedDate(date);
                }

                foreach (var intervalo in intervalosReservados)
                {
                    for (DateTime data = intervalo.dataInicio; data <= intervalo.dataFim; data = data.AddDays(1))
                    {
                        calendar.AddBoldedDate(data);
                    }
                }

                calendar.UpdateBoldedDates();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show($"Erro ao atualizar o calendário: Referência de objeto não definida para uma instância de um objeto.\nDetalhes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Tratar qualquer exceção que possa ocorrer e exibir uma mensagem de erro
                MessageBox.Show($"Ocorreu um erro ao atualizar o calendário: {ex.Message}",
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        //-------------------------------------------------------------------------

        public bool AgendarReserva(string cpf, DateTime dataCheckin, DateTime dataCheckout, string observacoes, int idChacara)
        {
            try
            {
                using (MySqlConnection conexao = new MySqlConnection(connectionStr))
                {


                    conexao.Open();
                    using (MySqlTransaction transacao = conexao.BeginTransaction())
                    {
                        // 1. Encontrar o idCliente a partir do CPF fornecido
                        int idCliente = CriarOuPegarIdCliente(cpf, conexao, transacao);

                        if (idCliente == -1)
                        {
                            MessageBox.Show("Cliente não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        // 2. Inserir a nova reserva na tabela
                        return InserirReserva(conexao, idCliente, dataCheckin, dataCheckout, observacoes, idChacara);
                    }    
                }
            }
            catch (MySqlException sqlEx)
            {
                // Exibir mensagem de erro específica para MySQL
                MessageBox.Show($"Erro ao conectar com o banco de dados: {sqlEx.Message}", "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                // Exibir mensagem de erro para outras exceções
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //-------------------------------------------------------------------------

        private bool InserirReserva(MySqlConnection conexao, int idCliente, DateTime dataCheckin, DateTime dataCheckout, string observacoes, int idChacara)
        {
            string query = @"
        INSERT INTO reserva (idCliente, dataInicio, dataFim, Observacoes, idChacara)
        VALUES (@idCliente, @dataInicio, @dataFim, @observacoes, @idChacara)";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@dataInicio", dataCheckin);
                    cmd.Parameters.AddWithValue("@dataFim", dataCheckout);
                    cmd.Parameters.AddWithValue("@observacoes", observacoes);
                    cmd.Parameters.AddWithValue("@idChacara", idChacara);

                    int resultado = cmd.ExecuteNonQuery();

                    // Verifica se a inserção foi bem-sucedida
                    return resultado > 0;
                }
            }
            catch (MySqlException sqlEx)
            {
                // Exibir mensagem de erro específica para MySQL
                MessageBox.Show($"Erro ao inserir a reserva no banco de dados: {sqlEx.Message}", "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                // Exibir mensagem de erro para outras exceções
                MessageBox.Show($"Erro inesperado ao inserir reserva: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //-------------------------------------------------------------------------

        private int CriarOuPegarIdCliente(string cpf, MySqlConnection connection, MySqlTransaction transaction)
        {
            // Consulta para verificar se o cliente já existe
            string query = @"
        SELECT idcliente 
        FROM cliente 
        WHERE cpf = @cpf";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                object result = cmd.ExecuteScalar();

                // Se o cliente já existe, retorna o idCliente encontrado
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            // Se o cliente não existir, cria um novo cliente
            query = @"
        INSERT INTO cliente (cpf) 
        VALUES (@cpf); 
        SELECT LAST_INSERT_ID();";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                object result = cmd.ExecuteScalar();

                // Retorna o idCliente recém-criado
                return Convert.ToInt32(result);
            }
        }

        //-------------------------------------------------------------------------

        public List<string> ObterNomesChacaras()
        {
            List<string> nomesChacaras = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionStr))
            {
                connection.Open();
                string query = "SELECT nomeChacara FROM chacara";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nomesChacaras.Add(reader.GetString("nomeChacara"));
                    }
                }
            }

            return nomesChacaras;
        }

        //-------------------------------------------------------------------------

        public bool ValidarInputs(string cliente, ComboBox comboBoxChacaras, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Verificar se o nome do cliente e a chácara foram selecionados
                if (string.IsNullOrEmpty(cliente) || comboBoxChacaras.SelectedIndex == -1)
                {
                    MessageBox.Show("O CPF do cliente e a seleção da chácara são obrigatórios.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Verificar se a data de início é uma data passada
                if (startDate < DateTime.Today)
                {
                    MessageBox.Show("Não é possível agendar para datas passadas. Por favor, selecione uma data a partir de hoje.", "Data Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Verificar se a data de fim é anterior à data de início
                if (endDate < startDate)
                {
                    MessageBox.Show("A data de fim não pode ser anterior à data de início. Por favor, selecione um intervalo válido.", "Data Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Verificar se o intervalo de datas excede 5 dias
                if (endDate.Subtract(startDate).Days > 5)
                {
                    MessageBox.Show("Não é possível agendar mais de 5 dias consecutivos. Por favor, selecione um intervalo menor.", "Intervalo Excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Caso todos os testes passem
                return true;
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro genérica em caso de exceções inesperadas
                MessageBox.Show($"Erro ao validar os dados: {ex.Message}", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //-------------------------------------------------------------------------

        // Método para obter o ID da chácara com base no nome
        public int ObterIdChacaraPorNome(string nomeChacara)
        {
            int idChacara = -1;

            using (MySqlConnection conexao = new MySqlConnection(connectionStr))
            {
                conexao.Open();

                string query = "SELECT idChacara FROM chacara WHERE nomeChacara = @nomeChacara";
                using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@nomeChacara", nomeChacara);

                    object resultado = cmd.ExecuteScalar();
                    if (resultado != null)
                    {
                        idChacara = Convert.ToInt32(resultado);
                    }
                }
            }
            return idChacara;
        }

        //-------------------------------------------------------------------------

     

        //-------------------------------------------------------------------------

        public bool ExcluirReserva(string nomeChacara, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStr))
                {
                    connection.Open();
                    string query = @"DELETE FROM reserva 
                             WHERE nomeChacara = @nomeChacara 
                             AND dataInicio = @dataInicio 
                             AND dataFim = @dataFim";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@nomeChacara", nomeChacara);
                    cmd.Parameters.AddWithValue("@dataInicio", dataInicio);
                    cmd.Parameters.AddWithValue("@dataFim", dataFim);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //-------------------------------------------------------------------------

        public bool AtualizarReserva(string nomeChacara, DateTime dataInicio, DateTime dataFim, string novasObservacoes)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStr))
                {
                    connection.Open();
                    string query = @"UPDATE reserva 
                             SET Observacoes = @novasObservacoes 
                             WHERE nomeChacara = @nomeChacara 
                             AND dataInicio = @dataInicio 
                             AND dataFim = @dataFim";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@novasObservacoes", novasObservacoes);
                    cmd.Parameters.AddWithValue("@nomeChacara", nomeChacara);
                    cmd.Parameters.AddWithValue("@dataInicio", dataInicio);
                    cmd.Parameters.AddWithValue("@dataFim", dataFim);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

