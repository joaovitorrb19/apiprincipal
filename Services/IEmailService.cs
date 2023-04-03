using ApiPrincipal.Model;

namespace ApiPrincipal.Services.Interfaces {
    public interface IEmailService {
        public Task EnviarEmailAsync(EmailModel EmailDestinatario);
    }
}