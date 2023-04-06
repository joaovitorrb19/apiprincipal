using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ApiPrincipal.Model {
    public class PedidoModel {
        [Key]
        public int PedidoId {get;set;}
        [Required]
        public string UserName {get;set;}

        [ForeignKey("Enderecos")]
        public int? EnderecoId {get;set;}

        public EnderecoModel? Endereco{get;set;}

        public double ValorTotal{get;set;}

        public string Situacao {get;set;}

        public List<ItemPedidoModel> ItensPedidos{get;set;} = new List<ItemPedidoModel>();
        
        public UsuarioModel? Usuario{get;set;}
        
        public DateTime? DataFechamento {get;set;}
 
    }
}