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

        public bool Ativa { get; set; } = true;

        public bool InscricoesAbertas {get; set;}= false;
    }
}