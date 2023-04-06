using ApiPrincipal.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Data {
    public class DataContext : DbContext {
            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {
                
            }

            public DbSet<CategoriaModel> Categorias {get;set;}

            public DbSet<ProdutoModel> Produtos{get;set;}

            public DbSet<PedidoModel> Pedidos{get;set;}

            public DbSet<EnderecoModel> Enderecos{get;set;}

            public DbSet<ItemPedidoModel> ItensPedidos {get;set;}
            
    }
}