using System.Text;
using ApiPrincipal.Model;

namespace ApiPrincipal.Services {
    public static class PopularEmailParaResetSenhaService {
        public static EmailModel PopularEmailParaResetSenha(string emailDestinatario,string Token){

                var email = new EmailModel();

                email.Assunto = "Redefinição de Senha";

                email.EmailDestinatario = emailDestinatario;

                var corpo = new StringBuilder();

                corpo.Append("<p>Clique no link para redefinir sua senha!</p>");
                corpo.Append($"<a href='{Token}'>Clique Aqui</a>");

                email.Body = corpo.ToString();

                return email;

        }   
    }
}