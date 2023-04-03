using System.ComponentModel.DataAnnotations;

namespace ApiPrincipal.ViewModel {
    public class RedefinirSenhaViewModel {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email{get;set;}
        [DataType(DataType.Password)]
        public string NovaSenha{get;set;}
        [DataType(DataType.Password)]
        [Compare("NovaSenha")]
        public string ConfNovaSenha{get;set;}

        public string Token {get;set;}
    }
}