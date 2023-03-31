using ApiPrincipal.Data;
using ApiPrincipal.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddDbContext<UsersDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDataContext")));
builder.Services.AddIdentity<UsuarioModel,IdentityRole>().AddEntityFrameworkStores<UsersDataContext>().AddDefaultTokenProviders();



var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseStaticFiles();

app.UseEndpoints(options => options.MapDefaultControllerRoute());

app.Run();
