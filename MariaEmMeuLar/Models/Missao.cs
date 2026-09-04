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

        [MaxLength(180)]
        public string? Resumo { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Slug { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemLogo { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemInscricao { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? ImagemFundo { get; set; } = string.Empty;

        public bool ExibirIndex { get; set; } = true;
        public bool ExibirInscricao { get; set; } = true;

        public int OrdemExibicao { get; set; } = 0;

    }
}