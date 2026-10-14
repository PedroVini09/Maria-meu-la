using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MariaEmMeuLar.Models
{
    [Table("missao_historia_tb")]
    public class MissaoHistoria
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public int MissaoId{get; set;}

        [Required(ErrorMessage ="Informe o título da historia.")]
        [MaxLength(150)]
        public string Titulo {get; set;} = string.Empty;

        [Required(ErrorMessage ="Informe o contéudo da historia.")]
        public string Conteudo {get; set;} = string.Empty;

        public DateTime DataAtualizacao {get; set;}

        [ForeignKey(nameof(MissaoId))]
        public Missao? Missao {get; set;}
    }
}