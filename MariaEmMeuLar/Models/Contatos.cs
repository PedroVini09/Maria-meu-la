using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class Contatos
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email não é um endereço de email válido.")]
        [MaxLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [Phone(ErrorMessage = "O campo Telefone não é um número de telefone válido.")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Assunto é obrigatório.")]
        [MaxLength(150)]
        public string Assunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Mensagem é obrigatória.")]
        public string Mensagem { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; } = DateTime.Now;

        public bool Respondido { get; set; } = false;
    }
}