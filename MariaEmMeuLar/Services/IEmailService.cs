using MariaEmMeuLar.Models.ViewModels;

namespace MariaEmMeuLar.Services;

public interface IEmailService
{
    Task EnviarMensagemContatoAsync(ContatoMensagemViewModel model);
}