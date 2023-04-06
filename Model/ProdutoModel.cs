using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiPrincipal.Model;

namespace ApiPrincipal.Model
{

    public class ProdutoModel
    {

        public ProdutoModel()
        {
            this.DataCadastro = DateTime.Now.Date;
        }
        [Key]
        public int IdProduto { get; set; }
        [Required]
        public double PrecoProduto{get;set;}
        [Required]
        public int? QuantidadeEstoque{get;set;}
        [Required]
        [MinLength(3,ErrorMessage = "Nome do produto muito curto")]
        [MaxLength(15,ErrorMessage = "Nome do produto muito extenso")]
        public string NomeProduto { get; set; }
        [Required]
        public string UsuarioQueCriou {get;set;}

        public DateTime? DataCadastro { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [ForeignKey("Categorias")]
        public int CategoriaId { get; set; }

        public CategoriaModel? Categoria { get; set; }
    }

}