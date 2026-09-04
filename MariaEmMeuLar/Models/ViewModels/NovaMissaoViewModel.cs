using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class NovaMissaoViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descricao { get; set; }

        [MaxLength(180)]
        public string? Resumo { get; set; }

        [Required(ErrorMessage = "O identificador da missão é obrigatório.")]
        [MaxLength(150)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemLogo { get; set; }

        [MaxLength(150)]
        public string? ImagemInscricao { get; set; }

        [MaxLength(150)]
        public string? ImagemFundo { get; set; }

        public bool Ativa { get; set; } = true;

        public bool InscricoesAbertas { get; set; } = false;

        public bool ExibirIndex { get; set; } = true;

        public bool ExibirInscricao { get; set; } = true;

        [Range(0, 100, ErrorMessage = "Informe uma ordem válida.")]
        public int OrdemExibicao { get; set; }
    }
}