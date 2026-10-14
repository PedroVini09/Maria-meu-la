using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class EditarItemSecaoViewModel
    {
        public int Id { get; set; }

        public int SecaoId { get; set; }

        public string SecaoTitulo { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Marcador { get; set; }

        [MaxLength(100)]
        public string? Icone { get; set; }

        [MaxLength(180)]
        public string? Titulo { get; set; }

        public string? Conteudo { get; set; }

        [MaxLength(200)]
        public string? Imagem { get; set; }

        [Range(0, 100)]
        public int Ordem { get; set; }

        public bool Ativo { get; set; }
    }
}