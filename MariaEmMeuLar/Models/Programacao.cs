using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MariaEmMeuLar.Models
{
    public class Programacao : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "O campo Título é obrigatório.")]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        public TimeSpan HoraInicial { get; set; }

        public TimeSpan HoraFinal { get; set; }


        [Required(ErrorMessage = "O campo Local é obrigatório.")]
        [MaxLength(200)]
        public string Local { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O campo Missão é obrigatório.")]
        public int? MissaoId { get; set; }

        public Missao? Missao { get; set; }

        public bool Ativa { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HoraInicial == TimeSpan.Zero)
            {
                yield return new ValidationResult("A hora inicial é obrigatória.", new[] { nameof(HoraInicial) });
            }
            if (HoraFinal == TimeSpan.Zero)
            {
                yield return new ValidationResult("A hora final é obrigatória.", new[] { nameof(HoraFinal) });
            }
            if( HoraInicial != TimeSpan.Zero && HoraFinal != TimeSpan.Zero && HoraFinal <= HoraInicial)
            {
                yield return new ValidationResult("A hora final deve ser maior que a hora inicial.", new[] { nameof(HoraFinal) });
            }
        }
    }
}