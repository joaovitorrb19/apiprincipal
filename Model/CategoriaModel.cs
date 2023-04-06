using System.ComponentModel.DataAnnotations;

namespace ApiPrincipal.Model {
    public class CategoriaModel {
        
        public CategoriaModel()
        {
            this.DataCadastro = DateTime.Now;
        }
        [Key]
        public int CategoriaId{get;set;}
        [Required(ErrorMessage = "Nome da Categoria Requerido...")]
        [MinLength(3,ErrorMessage = "Tamanho minimo 3 letras...")]
        [MaxLength(20,ErrorMessage = "Tamanho maximo 20 letras...")]
        public string NomeCategoria{get;set;}

        public DateTime DataCadastro{get;set;}

        public DateTime? DataAlteracao {get;set;}
        [Required]
        public string UsuarioQueCriou {get;set;}
        
    }
}