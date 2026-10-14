using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MariaEmMeuLar.Models
{
    [Table("missao_secao_tb")]
    public class MissaoSecao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MissaoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Chave { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Topico { get; set; } 

        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;
        public string? Conteudo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; } = "texto"; //  Valor Padrão é "texto"

        [MaxLength(200)]
        public string? Imagem { get; set; }

        public int Ordem { get; set; }

        public bool Ativa { get; set; } = true; 

        // Foreign key to Missao
        [ForeignKey(nameof(MissaoId))]
        public Missao? Missao { get; set; }
        public ICollection<MissaoSecaoItem> Itens { get; set; } = new List<MissaoSecaoItem>();
    }
}