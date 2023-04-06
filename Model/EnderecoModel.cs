using System.ComponentModel.DataAnnotations;

namespace ApiPrincipal.Model {

    public class EnderecoModel {
      [Key]
        public int EnderecoId {get;set;}
        [Required,MinLength(8),MaxLength(8)]
        public string cep {get;set;}
        [Required]
        public string? logradouro {get;set;}

        public string? complemento {get;set;}
        [Required]
        public string? bairro {get;set;}
        [Required]
        public string? localidade {get;set;}
        [Required]
        public string? uf {get;set;}
        [Required]
        public string? ddd {get;set;}
        [Required]
        public string UserName {get;set;}

    }

}
