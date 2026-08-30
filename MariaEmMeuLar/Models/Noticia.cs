using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class Noticia
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Resumo é obrigatório")]
        [MaxLength(300)]
        public string Resumo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Imagem é obrigatório.")]
        [MaxLength(255)]
        public string ImagemCapa { get; set; } = string.Empty;


        [Required(ErrorMessage = "O  Link da noticia  é obrigatório.")]
        [MaxLength(500)]
        public string LinkInstagram { get; set; } = string.Empty;

        
        public DateTime DataPublicacao { get; set; } = DateTime.Now;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Publicada { get; set; } = true;

        public bool Destaque { get; set; } = true;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um usuário administrador.")]
        public int UsuarioAdminId { get; set; }
        public UsuarioAdmin? UsuarioAdmin { get; set; }
    }
}