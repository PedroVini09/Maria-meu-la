using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class EditarInscricaoViewModel
    {
        public int Id { get; set; }

        public int MissaoId { get; set; }

        public string MissaoNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Range(10, 100, ErrorMessage = "Informe uma idade válida.")]
        public int? Idade { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Observacao { get; set; }

        [Required]
        public string Status { get; set; } = "Pendente";

    
        //campos especificos - Maria em Meu Lar
        [MaxLength(250)]
        public string? Endereco { get; set; }

        public DateTime? DataDesejada { get; set; }

        public TimeSpan? HorarioDesejado { get; set; }

        //campos específicos - Retiro Quaresmal / Segue-me
        [MaxLength(150)]
        public string? Comunidade { get; set; }

        [MaxLength(10)]
        public string? JaParticipou { get; set; }

        //campos específicos - Semana da Juventude
        [MaxLength(150)]
        public string? Grupo { get; set; }

        public string? Turno { get; set; }

        //campos específicos - terço da juventude
        public string? Participacao { get; set; }

        public string? DiaDisponivel { get; set; }
    }
}