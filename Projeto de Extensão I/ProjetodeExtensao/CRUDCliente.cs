using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetodeExtensao
{
    public class CRUDCliente
    {
        private string connectionStr;

        public CRUDCliente()
        {
            connectionStr = "Server=localhost;Database=reservas;User ID=root;Password=N@th$Fp197;";
        }

        public void inserirCliente(Cliente cliente, Telefone telefone, int idUsuario = 1)
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

                cmdInsert.Parameters.AddWithValue("rua", cliente.GetEndereco().GetRua());
                cmdInsert.Parameters.AddWithValue("numero", cliente.GetEndereco().GetNumero());
                cmdInsert.Parameters.AddWithValue("bairro", cliente.GetEndereco().GetBairro());
                cmdInsert.Parameters.AddWithValue("cidade", cliente.GetEndereco().GetCidade());
                cmdInsert.Parameters.AddWithValue("cep", cliente.GetEndereco().GetCEP());
                cmdInsert.Parameters.AddWithValue("complemento", cliente.GetEndereco().GetComplemento());

                cmdInsert.Parameters.Add("@idEndereco", MySqlDbType.Int32).Direction = ParameterDirection.Output;
                cmdInsert.ExecuteNonQuery();

                int idEndereco = Convert.ToInt32(cmdInsert.Parameters["@idEndereco"].Value);

                // Inserir o cliente
                cmdInsert.CommandText = "INSERT INTO cliente (nome, cpf, idEndereco, email, sexo, idUsuario) " +
                                        "VALUES (@nome, @cpf, @idEndereco, @email, @sexo, @idUsuario)";
                cmdInsert.Parameters.Clear();
                cmdInsert.Parameters.AddWithValue("nome", cliente.GetNome());
                cmdInsert.Parameters.AddWithValue("cpf", cliente.GetCPF());
                cmdInsert.Parameters.AddWithValue("idEndereco", idEndereco);
                cmdInsert.Parameters.AddWithValue("email", cliente.GetEmail());
                cmdInsert.Parameters.AddWithValue("sexo", cliente.GetSexo());
                cmdInsert.Parameters.AddWithValue("idUsuario", idUsuario); // Sempre 1

                cmdInsert.ExecuteNonQuery();

                // Inserir o telefone
                cmdInsert.CommandText = "INSERT INTO telefone (numero, idCliente) VALUES (@numero, LAST_INSERT_ID())";
                cmdInsert.Parameters.Clear();
                cmdInsert.Parameters.AddWithValue("numero", telefone.GetNumero());

                cmdInsert.ExecuteNonQuery();
            }
            finally
            {
                if (cmdInsert != null) cmdInsert.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }

        }

        // ----------------------------------------------------------------------------------------------------------------------------

        public Cliente pesquisarClientePorCPF(string cpf)
        {
            MySqlConnection connectionBD = null;
            MySqlCommand cmdSelect = null;
            Cliente cliente = null;

            try
            {
                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                cmdSelect = new MySqlCommand();
                cmdSelect.Connection = connectionBD;

                // Consulta SQL para recuperar os dados do cliente e o endereço associado
                cmdSelect.CommandText = "SELECT c.nome, c.cpf, e.rua, e.numero, e.bairro, e.cidade, e.cep, e.complemento, c.email, c.sexo " +
                                        "FROM cliente c " +
                                        "INNER JOIN endereco e ON c.idEndereco = e.idendereco " +
                                        "WHERE c.cpf = @cpf";

                cmdSelect.Parameters.AddWithValue("@cpf", cpf);

                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    string nome = reader.GetString(0);      // nome
                    string cpfCliente = reader.GetString(1); // cpf
                    Endereco endereco = new Endereco(
                        reader.GetString(2),  // rua
                        reader.GetInt32(3),   // numero
                        reader.GetString(4),  // bairro
                        reader.GetString(5),  // cidade
                        reader.GetString(6),  // cep
                        reader.GetString(7)   // complemento
                    );

                    // Cria um objeto Cliente com os dados recuperados
                    cliente = new Cliente(
                        nome,
                        cpfCliente,
                        endereco,
                        reader.GetString(8),  // email
                        reader.GetString(9)   // sexo
                    );
                }

                reader.Close();
            }
            finally
            {
                if (cmdSelect != null) cmdSelect.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }

            return cliente;
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        public bool removerCliente(string cpfCliente)
        {
            int numLinhasAfetadas = 0;
            MySqlConnection connectionBD = null;
            MySqlCommand cmdDelete = null;

            try
            {
                // Estabelece a conexão com o banco de dados
                connectionBD = new MySqlConnection(connectionStr);
                connectionBD.Open();

                // Inicia uma transação para garantir que todas as operações sejam atômicas
                using (var transaction = connectionBD.BeginTransaction())
                {
                    try
                    {
                        // Remove registros na tabela telefone associados ao cliente
                        using (var cmdDeleteTelefones = new MySqlCommand("DELETE FROM telefone WHERE idCliente = (SELECT idCliente FROM cliente WHERE cpf = @cpfCliente)", connectionBD, transaction))
                        {
                            cmdDeleteTelefones.Parameters.AddWithValue("@cpfCliente", cpfCliente);
                            cmdDeleteTelefones.ExecuteNonQuery();
                        }

                        // Remove registros na tabela reservas associados ao cliente
                        using (var cmdDeleteReservas = new MySqlCommand("DELETE FROM reserva WHERE idCliente = (SELECT idCliente FROM cliente WHERE cpf = @cpfCliente)", connectionBD, transaction))
                        {
                            cmdDeleteReservas.Parameters.AddWithValue("@cpfCliente", cpfCliente);
                            cmdDeleteReservas.ExecuteNonQuery();
                        }

                        // Remove o cliente
                        cmdDelete = new MySqlCommand("DELETE FROM cliente WHERE cpf = @cpfCliente", connectionBD, transaction);
                        cmdDelete.Parameters.AddWithValue("@cpfCliente", cpfCliente);
                        numLinhasAfetadas = cmdDelete.ExecuteNonQuery();

                        // Confirma a transação
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Reverte a transação em caso de erro
                        transaction.Rollback();
                        throw; // Re-levanta a exceção para tratamento posterior
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Trata erros relacionados ao banco de dados
                MessageBox.Show("Erro ao remover cliente: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Trata erros genéricos
                MessageBox.Show("Erro desconhecido: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Libera recursos
                if (cmdDelete != null) cmdDelete.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }

            // Retorna verdadeiro se pelo menos uma linha foi afetada (ou seja, cliente removido)
            return numLinhasAfetadas > 0;
        }



        // ----------------------------------------------------------------------------------------------------------------------------

        public bool atualizarCliente(string cpfOriginal, Cliente novoCliente)
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
                                        "complemento = @complemento " +
                                        "WHERE idendereco = (SELECT idEndereco FROM cliente WHERE cpf = @cpfOriginal)";

                cmdUpdate.Parameters.AddWithValue("@rua", novoCliente.GetEndereco().GetRua());
                cmdUpdate.Parameters.AddWithValue("@numero", novoCliente.GetEndereco().GetNumero());
                cmdUpdate.Parameters.AddWithValue("@bairro", novoCliente.GetEndereco().GetBairro());
                cmdUpdate.Parameters.AddWithValue("@cidade", novoCliente.GetEndereco().GetCidade());
                cmdUpdate.Parameters.AddWithValue("@cep", novoCliente.GetEndereco().GetCEP());
                cmdUpdate.Parameters.AddWithValue("@complemento", novoCliente.GetEndereco().GetComplemento());
                cmdUpdate.Parameters.AddWithValue("@cpfOriginal", cpfOriginal);

                numLinhasAfetadas += cmdUpdate.ExecuteNonQuery();

                // Atualizar o cliente
                cmdUpdate.CommandText = "UPDATE cliente SET " +
                                        "nome = @nome, " +
                                        "cpf = @cpf, " +
                                        "email = @email, " +
                                        "sexo = @sexo " +
                                        "WHERE cpf = @cpfOriginal";

                cmdUpdate.Parameters.Clear();
                cmdUpdate.Parameters.AddWithValue("@nome", novoCliente.GetNome());
                cmdUpdate.Parameters.AddWithValue("@cpf", novoCliente.GetCPF()); // Pode ser o mesmo CPF ou um novo CPF, dependendo do caso
                cmdUpdate.Parameters.AddWithValue("@email", novoCliente.GetEmail());
                cmdUpdate.Parameters.AddWithValue("@sexo", novoCliente.GetSexo());
                cmdUpdate.Parameters.AddWithValue("@cpfOriginal", cpfOriginal);

                numLinhasAfetadas += cmdUpdate.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // Exibe o erro no console ou registre o erro conforme necessário
                Console.WriteLine("Erro ao atualizar o cliente: " + ex.Message);
                return false;
            }
            finally
            {
                if (cmdUpdate != null) cmdUpdate.Dispose();
                if (connectionBD != null) connectionBD.Close();
            }

            return numLinhasAfetadas > 0;
        }
    }
}





