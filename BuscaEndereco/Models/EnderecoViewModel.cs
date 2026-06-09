using System.ComponentModel.DataAnnotations;

namespace BuscaEndereco.Models
{
    public class EnderecoViewModel
    {
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [RegularExpression(@"^\d{5}-\d{3}$|^\d{8}$", ErrorMessage = "Formato de CEP inválido.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        [Display(Name = "Rua/Logradouro")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O estado (UF) é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter exatamente 2 letras.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O número do endereço é obrigatório.")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }
    }
}
