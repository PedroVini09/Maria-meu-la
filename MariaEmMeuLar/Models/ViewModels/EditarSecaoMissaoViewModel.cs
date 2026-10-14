using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class EditarSecaoMissaoViewModel
    {
        public int Id { get; set; }

        public int MissaoId { get; set; }

        public string MissaoNome { get; set; } = string.Empty;


        [Required]
        [MaxLength(100)]
        public string Chave { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? Topico { get; set; }


        [Required]
        [MaxLength(180)]
        public string Titulo { get; set; } = string.Empty;


        public string? Conteudo { get; set; }


        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = "texto";


        [MaxLength(200)]
        public string? Imagem { get; set; }


        [Range(0, 100)]
        public int Ordem { get; set; }


        public bool Ativa { get; set; }
    }
}