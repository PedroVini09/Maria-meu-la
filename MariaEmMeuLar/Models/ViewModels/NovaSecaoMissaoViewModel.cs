using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class NovaSecaoMissaoViewModel
    {
        [Required]
        public int MissaoId { get; set; }

        public string MissaoNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a chave da seção.")]
        [MaxLength(100)]
        public string Chave { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Topico { get; set; }

        [Required(ErrorMessage = "Informe o título da seção.")]
        [MaxLength(180)]
        public string Titulo { get; set; } = string.Empty;

        public string? Conteudo { get; set; }

        [Required(ErrorMessage = "Informe o tipo da seção.")]
        [MaxLength(50)]
        public string Tipo { get; set; } = "texto"; // Valor Padrão é "texto"

        [MaxLength(200)]
        public string? Imagem { get; set; }

        public int Ordem { get; set; }

        public bool Ativa { get; set; } = true;
    }
}