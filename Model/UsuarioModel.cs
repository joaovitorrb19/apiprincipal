using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ApiPrincipal.Model {

    public class UsuarioModel : IdentityUser{

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
        
    }

}