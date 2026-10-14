using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MariaEmMeuLar.Models
{
    [Table("missao_secao_item_tb")]
    public class MissaoSecaoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SecaoId { get; set; }

        public string? Marcador { get; set; }

        public string? Icone { get; set; }

        public string? Titulo { get; set; }

        public string? Conteudo { get; set; }

        public string? Imagem { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; } = true;

        [ForeignKey(nameof(SecaoId))]
        public MissaoSecao? Secao { get; set; }
    }
}