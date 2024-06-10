using Microsoft.Data.SqlClient;

namespace Teste7Comm.API.Data
{
    public class Command
    {
        private readonly string _connectionString;

        public Command(string connectionString)
        {
            _connectionString = connectionString;
            ExecuteCreateTables();
        }

        public SqlConnection OpenConnection(SqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Closed)
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                return connection;
            }
            return connection;
        }
        public SqlConnection CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                return connection;
            }
            return connection;
        }
        public async Task<bool> ExecuteInsert(string tabelaNome, object parametros)
        {
            SqlConnection connection = new SqlConnection();
            try
            {

                string cmdInsert = $"Insert {tabelaNome} (colunas) values (";
                SqlParameter[] sqlParameters = new SqlParameter[parametros.GetType().GetProperties().Length];
                int i = 0;
                string colunas = "";
                // Itera sobre as propriedades do objeto de parâmetros
                foreach (var propriedade in parametros.GetType().GetProperties())
                {
                    // Adiciona o nome da coluna à string de comando SQL
                    if (i == parametros.GetType().GetProperties().Length - 1)
                    {
                        cmdInsert += "@" + propriedade.Name;
                        colunas += propriedade.Name;
                    }
                    else
                    {
                        cmdInsert += "@" + propriedade.Name + ", ";
                        colunas += propriedade.Name + ",";
                    }

                    // Adiciona o parâmetro ao array de parâmetros
                    sqlParameters[i] = new SqlParameter("@" + propriedade.Name, propriedade.GetValue(parametros));
                    i++;
                }
                cmdInsert = cmdInsert.Replace("colunas", colunas);
                cmdInsert += ")";
                using (connection = OpenConnection(connection))
                {
                    using (SqlCommand command = new SqlCommand(cmdInsert, connection))
                    {
                        command.Parameters.AddRange(sqlParameters);
                        await command.ExecuteNonQueryAsync();
                    }
                    CloseConnection(connection);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        private async void ExecuteCreateTables()
        {
            SqlConnection connection = new SqlConnection();

            var querysTable = "Create Table Pessoa" +
                "(" +
                "idPessoa int primary key identity(1,1)," +
                "Nome varchar(150)," +
                "Email varchar(150)," +
                "Telefone varchar(50)," +
                "Cpf varchar(14)," +
                "dthNascimento DateTime" +
                ")" +
                "Create Table Endereco" +
                "(" +
                "idEndereco int primary key identity(1,1)," +
                "cep varchar(50)," +
                "logradouro varchar(150)," +
                "numero int," +
                "complemento varchar(150)," +
                "bairro varchar(100)," +
                "localidade varchar(150)," +
                "uf varchar(4)," +
                "idPessoa int," +
                "constraint fk_idPessoa_Pessoa foreign key (idPessoa) references Pessoa(idPessoa)" +
                ")";
            try
            {

                using (connection = OpenConnection(connection))
                {
                    using (var cmd = new SqlCommand(querysTable, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    CloseConnection(connection);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

    }
}
