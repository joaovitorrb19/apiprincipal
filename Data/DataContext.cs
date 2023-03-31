using ApiPrincipal.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Data {
    public class DataContext : DbContext {
            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {
                
            }

            public DbSet<CategoriaModel> Categorias {get;set;}
    }
}