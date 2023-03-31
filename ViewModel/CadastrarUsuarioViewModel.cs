using System.ComponentModel.DataAnnotations;

namespace ApiPrincipal.ViewModel {
    public class CadastrarUsuarioViewModel {

        [Required(ErrorMessage = "E-mail requerido...")]
        [DataType(DataType.EmailAddress)]
        public string Email{get;set;}

        [Required(ErrorMessage = "CPF requerido...")]
        [StringLength(11,ErrorMessage = "Tamanho do campo Invalido...")]
        public string CPF {get;set;}

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefone {get;set;}

        [Required(ErrorMessage = "Nome Completo requerido...")]
        [DataType(DataType.Text)]
        [MinLength(6)]
        [MaxLength(25)]
        public string NomeCompleto{get;set;}
        [Required(ErrorMessage = "Data de Nascimento requerida...")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento {get;set;}

        [Required(ErrorMessage = "Senha requerida...")]
        [DataType(DataType.Password)]
        public string Senha{get;set;}

        [Required(ErrorMessage = "Confirmação de Senha requerida...")]
        [DataType(DataType.Password)]
        [Compare("Senha")]
        public string ConfirmacaoSenha{get;set;}

    }
}