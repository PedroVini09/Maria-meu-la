using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class Programacao
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "O campo Título é obrigatório.")]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;


        public TimeSpan HoraInicial { get; set; } = TimeSpan.Zero;

        public TimeSpan HoraFinal { get; set; } = TimeSpan.Zero;

        [Required(ErrorMessage = "O campo Local é obrigatório.")]
        [MaxLength(200)]
        public string Local { get; set; } = string.Empty;

        [Required]
        public int? MissaoId { get; set; }

        public Missao? Missao { get; set; }

        public bool Ativa { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}