using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class UsuarioAdmin
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

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Perfil { get; set; } = "Admin";

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? UltimoAcesso { get; set; } 
    }
}