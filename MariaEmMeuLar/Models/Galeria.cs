using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class Galeria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Caminho da Imagem é obrigatório.")]
        [MaxLength(255)]
        public string CaminhoImagem { get; set; } = string.Empty;

        public DateTime DataPublicacao { get; set; } = DateTime.Now;

        public bool Ativa { get; set; } = true;

        
        public int MissaoId { get; set; }
        public Missao? Missao { get; set; }
    }
}