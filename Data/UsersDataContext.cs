using ApiPrincipal.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Data {

    public class UsersDataContext : IdentityDbContext<UsuarioModel> {

        public UsersDataContext(DbContextOptions<UsersDataContext> options) : base(options)
        {
            
        }

    }
    
}