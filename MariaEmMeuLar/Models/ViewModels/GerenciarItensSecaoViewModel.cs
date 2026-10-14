namespace MariaEmMeuLar.Models.ViewModels
{
    public class GerenciarItensSecaoViewModel
    {
        public MissaoSecao Secao { get; set; } = null!;

        public List<MissaoSecaoItem> Itens { get; set; }
            = new List<MissaoSecaoItem>();
    }
}