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

        [Required(ErrorMessage = "O campo Idade é obrigatório.")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
        [MaxLength(120)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

       [Required]
       public int MissaoId { get; set; }

       public Missao? Missao { get; set; }

       
        [MaxLength(500)]
        public string? Observacao { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = "Pendente";

        public DateTime DataInscricao { get; set; } = DateTime.Now;
    }
}