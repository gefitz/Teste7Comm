namespace Teste7Comm.API.Model
{
    public class EnderecoModel
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public int numero { get; set; }
    }
    public class EnderecoInsertModel
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public int idPessoa { get; set; }
        public int numero { get; set; }

    }
}
