
using ApiPrincipal.Model;
using ApiPrincipal.Services.Interfaces;
using ApiPrincipal.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ApiPrincipal.Services
{

    public class EmailService : IEmailService
    {

        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task EnviarEmailAsync(EmailModel EmailDestinatario)
        {

            var emailConfig = new MimeMessage();
            emailConfig.From.Add( new MailboxAddress(_emailSettings.NomeRemetente,_emailSettings.EmailRemetente));
            emailConfig.To.Add(MailboxAddress.Parse(EmailDestinatario.EmailDestinatario));
            emailConfig.Subject = EmailDestinatario.Assunto;

            var BodyEmailBuilder = new BodyBuilder{
                TextBody = string.Empty,
                HtmlBody = EmailDestinatario.Body
            };

            
            emailConfig.Body = BodyEmailBuilder.ToMessageBody();

            try{
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Porta);
            await smtp.AuthenticateAsync(_emailSettings.EmailRemetente, _emailSettings.Senha);
            var res =  await smtp.SendAsync(emailConfig);
            await smtp.DisconnectAsync(true);
            } catch (Exception e){
                throw new InvalidOperationException(e.Message);
            }
        }


    }

}