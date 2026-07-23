using System.Net;
using System.Net.Mail;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace MariaEmMeuLar.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    
    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task EnviarMensagemContatoAsync(ContatoMensagemViewModel model)
    {
       string nome = WebUtility.HtmlEncode(model.Nome);
       string email = WebUtility.HtmlEncode(model.Email);
       string telefone = WebUtility.HtmlEncode(model.Telefone ?? "Não Informado");
       string assunto = WebUtility.HtmlEncode(model.Assunto);
       string mensagemTexto = WebUtility.HtmlEncode(model.Mensagem);
       
       string assuntoEmail = $"Nova Mensagem do site - {assunto}";
       
       string corpoEmail = $@"
            <h2>Nova mensagem recebida pelo site</h2>

            <p><strong>Nome:</strong> {nome}</p>
            <p><strong>E-mail:</strong> {email}</p>
            <p><strong>Telefone:</strong> {telefone}</p>
            <p><strong>Assunto:</strong> {assunto}</p>

            <hr />

            <p><strong>Mensagem:</strong></p>
            <p>{mensagemTexto}</p>
        ";
        
        using var mensagem = new MailMessage();
        
        mensagem.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
        mensagem.To.Add(_settings.ReceiverEmail);
        mensagem.Subject = assuntoEmail;
        mensagem.Body = corpoEmail;
        mensagem.IsBodyHtml = true;

        mensagem.ReplyToList.Add(new MailAddress(model.Email, model.Nome));
        
        using var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
            EnableSsl = true
        };
        await smtp.SendMailAsync(mensagem);
    }
}