using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPrincipal.Model {
    public class ItemPedidoModel {
        [Key]
        public int ItemPedidoId{get;set;}
        [ForeignKey("Pedidos")]
        public int PedidoId{get;set;}
        [ForeignKey("Produtos")]
        public int ProdutoId{get;set;}

        public ProdutoModel? Produto {get;set;}
        
        [Required]
        public int Quantidade{get;set;}
    }
}