using System.ComponentModel.DataAnnotations;

namespace Teste7Comm.Web.Model
{
    public class EnderecoModel
    {
        [Required]
        [Display(Name ="CEP")]
        public string cep { get; set; }
        [Required]
        [Display(Name = "Rua")]
        public string logradouro { get; set; }
        [Display(Name = "Complemento")]
        public string complemento { get; set; }
        [Required]
        [Display(Name = "Bairro")]
        public string bairro { get; set; }
        [Required]
        [Display(Name = "Cidade")]
        public string localidade { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string uf { get; set; }
        [Required]
        [Display(Name = "Número")]
        public int numero { get; set; }
    }
}