using System.ComponentModel.DataAnnotations;

namespace ApiPrincipal.ViewModel{
    public class LoginViewModel {
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Senha {get;set;} 

        
        
        public string? returnUrl{get;set;}
    }
}