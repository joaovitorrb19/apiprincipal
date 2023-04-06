using ApiPrincipal.Data;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories;
using ApiPrincipal.Repositories.Interfaces;
using ApiPrincipal.Services;
using ApiPrincipal.Services.Interfaces;
using ApiPrincipal.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddDbContext<UsersDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDataContext")));

builder.Services.AddIdentity<UsuarioModel,IdentityRole>(
    options => {
        options.User.RequireUniqueEmail = true ;
        options.Password.RequireDigit = true ;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false ;
        options.Password.RequiredUniqueChars =  2 ;
        options.Password.RequireNonAlphanumeric = false ;
        options.SignIn.RequireConfirmedEmail = true;

    }
).AddEntityFrameworkStores<UsersDataContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(
    options => {
        options.LoginPath = "/usuario/login";
        options.LogoutPath = "/usuario/login";
        options.ReturnUrlParameter = "returnUrl";
        options.AccessDeniedPath = "/usuario/acessorestrito";
        options.Cookie.Name = "apiprincipal";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
        });

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IBaseRepository<CategoriaModel>,CategoriaRepository>();
builder.Services.AddScoped<IBaseRepository<PedidoModel>,PedidoRepository>();
builder.Services.AddScoped<IBaseRepository<ProdutoModel>,ProdutoRepository>();
builder.Services.AddScoped<IBaseRepository<EnderecoModel>,EnderecoRepository>();
builder.Services.AddScoped<IBaseRepository<ItemPedidoModel>,ItemPedidoRepository>();

builder.Services.AddScoped<DataContext,DataContext>();

builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name:"default",
    pattern:"{controller=usuario}/{action=login}/{id?}"
);

app.Run();
