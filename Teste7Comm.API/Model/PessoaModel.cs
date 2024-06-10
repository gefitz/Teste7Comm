namespace Teste7Comm.API.Model
{
    public class PessoaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set;}
        public string Telefone { get; set;}
        public string CPF { get; set;}
        public DateTime dthNascimento { get; set;}
        public EnderecoModel Endereco { get; set; }
    }
    public class PessoaInsertModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public DateTime dthNascimento { get; set; }
    }
}
