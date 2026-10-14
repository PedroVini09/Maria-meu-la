using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels
{
    public class EditarHistoriaMissaoViewModel
    {
        public int MissaoId {get; set;}

        public string MissaoNome {get; set;}= string.Empty;

        [Required(ErrorMessage = "Informe o título da historia.")]
        [MaxLength(150)]
        public string Titulo {get; set;} = string.Empty;

        [Required(ErrorMessage = "Informe o conteúdo da historia.")]
        public string Conteudo {get; set;} = string.Empty;
    }
}