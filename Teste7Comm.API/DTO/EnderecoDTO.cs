using System.ComponentModel.DataAnnotations;

namespace Teste7Comm.API.DTO
{
    public class EnderecoDTO
    {
        [Required]
        [Display(Name = "CEP")]
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
        [Display(Name = "Número")]
        public int numero { get; set; }
    }
}