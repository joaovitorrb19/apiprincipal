var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseStaticFiles();

app.UseEndpoints(options => options.MapDefaultControllerRoute());

app.Run();
