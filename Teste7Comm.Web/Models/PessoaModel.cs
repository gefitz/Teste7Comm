using System.ComponentModel.DataAnnotations;

namespace Teste7Comm.Web.Model
{
    public class PessoaModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name ="Nome")]
        public string nome { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string email { get; set;}
        [Required]
        [Display(Name = "Teleofone")]
        public string telefone { get; set;}
        [Required]
        [Display(Name = "CPF")]
        public string cpf { get; set;}
        [Display(Name = "Data de Nascimento")]
        public DateTime dthNascimento { get; set;}
        public EnderecoModel endereco { get; set; }
    }
}
