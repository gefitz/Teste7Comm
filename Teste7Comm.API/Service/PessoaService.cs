using Microsoft.Data.SqlClient;
using System.Text.Json;
using Teste7Comm.API.Data;
using Teste7Comm.API.Model;

namespace Teste7Comm.API.Service
{
    public class PessoaService
    {
        private readonly Command _command;

        public PessoaService(Command command)
        {
            _command = command;
        }
        public async Task<EnderecoModel> BuscaEnderecoCEP(string cep)
        {
            EnderecoModel endereco = new EnderecoModel();
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        endereco = JsonSerializer.Deserialize<EnderecoModel>(responseData);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return endereco;
        }
        public async Task<PessoaModel> CadastrarPessoa(PessoaModel pessoa)
        {
            PessoaModel ret = new PessoaModel();
            PessoaInsertModel pessoaInsert = new PessoaInsertModel()
            {
                CPF = pessoa.CPF,
                dthNascimento = pessoa.dthNascimento,
                Email = pessoa.Email,
                Nome = pessoa.Nome,
                Telefone = pessoa.Telefone
            };
            if (await _command.ExecuteInsert("Pessoa", pessoaInsert))
            {
                var resgatePessoaPraId = await BuscaPessoa(pessoa.Nome, pessoa.CPF);
                ret = resgatePessoaPraId.FirstOrDefault();
                if (ret != null)
                {
                    pessoa.Id = ret.Id;
                    if (await InsereEndereco(pessoa.Endereco, pessoa.Id)) return ret;
                    else return null;
                }
            }
            return ret;


        }
        public async Task<IEnumerable<PessoaModel>> BuscaPessoa(string nome, string cpf)
        {
            List<PessoaModel> pessoaList = new List<PessoaModel>();
            string query = $"Select * from Pessoa where (@nome ='' or nome = @nome) and (@cpf ='' or cpf = @cpf)";
            SqlConnection connection = new SqlConnection();
            SqlParameter[] parameter = new SqlParameter[2];
            parameter[0] = new SqlParameter("@nome", nome == null ? "" : nome);
            parameter[1] = new SqlParameter("@cpf", cpf == null ? "" : cpf);
            try
            {
                using (connection = _command.OpenConnection(connection))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameter);
                        var ret = await command.ExecuteReaderAsync();
                        while (ret.Read())
                        {
                            PessoaModel p = new PessoaModel()
                            {
                                CPF = Convert.ToString(ret["Cpf"]),
                                dthNascimento = Convert.ToDateTime(ret["dthNascimento"]),
                                Email = Convert.ToString(ret["Email"]),
                                Nome = Convert.ToString(ret["Nome"]),
                                Id = Convert.ToInt32(ret["idPessoa"]),
                                Telefone = Convert.ToString(ret["Telefone"])
                            };
                            pessoaList.Add(p);
                        }
                    }
                }
                for (int i = 0; i < pessoaList.Count(); i++)
                {
                    var endereco = await BuscaEndereco(pessoaList[i].Id);
                    if (endereco != null) pessoaList[i].Endereco = endereco;
                }
            }
            catch (Exception ex) { return null; }


            return pessoaList;
        }

        public bool RemoverPessoa(int id)
        {
            SqlConnection conn = new SqlConnection();
            using (conn = _command.OpenConnection(conn))
            {
                string query = "Delete from Pessoa where idPessoa = " + id;
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public async Task<bool> UpdatePessoa(PessoaModel pessoa)
        {
            string query = "Update Pessoa set " +
                "Nome = '" + pessoa.Nome + "', " +
                "Email = '" + pessoa.Email + "', " +
                "Telefone = '" + pessoa.Telefone + "', " +
                "Cpf = '" + pessoa.CPF + "', " +
                "dthNascimento = '" + pessoa.dthNascimento + "' " +
                "where idPessoa = " + pessoa.Id +

                " Update Endereco set " +
                "Cep = '" + pessoa.Endereco.cep + "', " +
                "logradouro = '" + pessoa.Endereco.logradouro + "', " +
                "complemento = '" + pessoa.Endereco.complemento + "', " +
                "bairro = '" + pessoa.Endereco.bairro + "', " +
                "localidade = '" + pessoa.Endereco.localidade + "', " +
                "uf = '" + pessoa.Endereco.uf + "', " +
                "numero = " + pessoa.Endereco.numero +
                " where idPessoa = " + pessoa.Id;

            SqlConnection conn = new SqlConnection();
            using(conn = _command.OpenConnection(conn))
            {
                using(SqlCommand cmd = new SqlCommand(query,conn)) 
                {
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }

        } 
        private async Task<bool> InsereEndereco(EnderecoModel item, int idPessoa)
        {


            EnderecoInsertModel endereco = new EnderecoInsertModel()
            {
                bairro = item.bairro,
                cep = item.cep,
                complemento = item.complemento,
                idPessoa = idPessoa,
                localidade = item.localidade,
                logradouro = item.logradouro,
                uf = item.uf
            };
            if (await _command.ExecuteInsert("Endereco", endereco))
            {
                return true;


            }
            else
            {
                return false;
            }

        }
        private async Task<EnderecoModel> BuscaEndereco(int idPessoa)
        {
            EnderecoModel endereco = new EnderecoModel();
            string query = $"Select * from Endereco where idPessoa = {idPessoa}";

            SqlConnection connection = new SqlConnection();
            try
            {
                using (connection = _command.OpenConnection(connection))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var ret = await command.ExecuteReaderAsync();
                        while (ret.Read())
                        {
                            endereco = new EnderecoModel()
                            {
                                bairro = ret["bairro"].ToString(),
                                cep = ret["cep"].ToString(),
                                complemento = ret["complemento"].ToString(),
                                uf = ret["uf"].ToString(),
                                localidade = ret["localidade"].ToString(),
                                logradouro = ret["logradouro"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex) { return null; }


            return endereco;
        }

    }
}
