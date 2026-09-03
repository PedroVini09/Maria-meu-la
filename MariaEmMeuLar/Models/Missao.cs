using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models
{
    public class Missao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;


        [MaxLength(500)]
        public string? Descricao { get; set; } = string.Empty;

        public bool Ativa { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();

        public ICollection<Programacao> Programacoes { get; set; } = new List<Programacao>();

        public ICollection<Galeria> Galerias { get; set; } = new List<Galeria>();

        public bool InscricoesAbertas { get; set; } = true;
    }
}