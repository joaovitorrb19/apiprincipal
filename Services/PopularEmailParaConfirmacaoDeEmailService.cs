using System.Text;
using ApiPrincipal.Model;

namespace ApiPrincipal.Services {
    public static class PopularEmailParaConfirmacaoDeEmailService {
        public static EmailModel PopularEmailParaConfirmacaoDeEmail(string emailDestinatario,string Token){

                var email = new EmailModel();

                email.Assunto = "Confirmação de Email";

                email.EmailDestinatario = emailDestinatario;

                var corpo = new StringBuilder();

                corpo.Append("<p>Clique no link para confirmar o seu E-mail!</p>");
                corpo.Append($"<a href='{Token}'>Clique Aqui</a>");

                email.Body = corpo.ToString();

                return email;

        }   
    }
}