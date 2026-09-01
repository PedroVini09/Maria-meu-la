using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class Inscricao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Range(10,100, ErrorMessage = "Informe uma idade válida.")]
        public int? Idade { get; set; }

        
        // [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
        // [MaxLength(120)]
        // public string? Email { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

       
       [Range(1, int.MaxValue, ErrorMessage = "O campo Missão é obrigatório.")]
       public int MissaoId { get; set; }

       public Missao? Missao { get; set; }

       
        [MaxLength(500)]
        public string? Observacao { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = "Pendente";

        public DateTime DataInscricao { get; set; } = DateTime.Now;

        //campos especificos - Maria em Meu Lar
        [MaxLength(250)]
        public string? Endereco {get; set;}

        public DateTime? DataDesejada {get; set;}

        public TimeSpan? HorarioDesejado {get; set;}

        //campos específicos - Retiro Quaresmal

        [MaxLength(150)]
        public string? Comunidade {get; set;}

        [MaxLength(10)]
        public string? JaParticipou {get; set;}

        //campos específicos - Semana da Juventude
        [MaxLength(150)]
        public string? Grupo {get; set;}

        [MaxLength(20)]
        public string? Turno {get; set;}

        //campos específicos - Terço da Juventude
        [MaxLength(30)]
        public string? Participacao{get; set;}

        [MaxLength(20)]
        public string? DiaDisponivel {get; set;}
    }
}