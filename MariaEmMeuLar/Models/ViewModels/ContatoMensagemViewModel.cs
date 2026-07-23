using System.ComponentModel.DataAnnotations;

namespace MariaEmMeuLar.Models.ViewModels;

public class ContatoMensagemViewModel
{
    [Required(ErrorMessage = "nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Numero de telefone é obrigatorio")]
    public string? Telefone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Selecione o assunto")]
    public string Assunto { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Digite a mensagem principal da conversa")]
    public string Mensagem { get; set; } = string.Empty;
}