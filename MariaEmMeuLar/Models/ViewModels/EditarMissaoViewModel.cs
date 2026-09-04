using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class EditarMissaoViewModel
    {
       
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descricao { get; set; } = string.Empty;

        [MaxLength(180)]
        public string? Resumo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O identificador da missão é obrigatório.")]
        [MaxLength(150)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemLogo { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemInscricao { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemFundo { get; set; } = string.Empty;

        public bool Ativa { get; set; } 

        public bool ExibirIndex { get; set; }

        public bool ExibirInscricao { get; set; } 

        [Range(0, 100, ErrorMessage = "Informe uma ordem válida.")]
        public int OrdemExibicao { get; set; } 
    }
}