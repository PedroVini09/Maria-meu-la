namespace MariaEmMeuLar.Models.ViewModels
{
    public class GerenciarConteudoMissaoViewModel
    {
        public Missao Missao { get; set; } = null!;

        public List<MissaoSecao> Secoes { get; set; } = new List<MissaoSecao>();

    }
}