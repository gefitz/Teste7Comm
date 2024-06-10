
using System.ComponentModel.DataAnnotations;
using Teste7Comm.API.Model;

namespace Teste7Comm.API.DTO
{
    public class PessoaDTO
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string nome { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Teleofone")]
        public string telefone { get; set; }
        [Required]
        [Display(Name = "CPF")]
        public string cpf { get; set; }
        [Display(Name = "Data de Nascimento")]
        public DateTime dthNascimento { get; set; }
        public EnderecoModel endereco { get; set; }
    }
}
