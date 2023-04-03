using System.ComponentModel.DataAnnotations;

namespace ApiPrincipal.ViewModel {
    public class EsqueciSenhaViewModel {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email{get;set;}

    }
}