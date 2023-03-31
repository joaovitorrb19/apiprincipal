using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers{

    public class UsuarioController : Controller{

        private readonly UserManager<UsuarioModel> _UserManager;

        private readonly SignInManager<UsuarioModel> _SignInManager;

        public UsuarioController(UserManager<UsuarioModel> UserManager, SignInManager<UsuarioModel> signInManager)
        {
            _UserManager = UserManager;

            _SignInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Login(string? returnUrl){
            if(String.IsNullOrEmpty(returnUrl)){
                return View();
            } else {
                return View(new LoginViewModel{returnUrl = returnUrl});
            } 
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm]LoginViewModel login){

            if(ModelState.IsValid){
                var resultado = await _SignInManager.PasswordSignInAsync(login.Email,login.Senha,false,false);
                if(resultado.Succeeded){
                    this.MostrarMensagem("Bem vindo!");
                    if(String.IsNullOrEmpty(login.returnUrl)){
                        return RedirectToAction("Index","Home");
                    } else {
                        return RedirectToAction("Index","Home");
                    }
                } else {
                    this.MostrarMensagem("Usuario ou senha incorretos...",TipoMensagem.Erro);
                    return View(login);
                }
            } else {
                this.MostrarMensagem("Credenciais inválidas, verifique os campos...",TipoMensagem.Erro);
                return View(login);
            }
        }

        [HttpGet]
        public ActionResult AcessoRestrito(string? returnUrl){
            return View();
        }

        [HttpGet]
        public ActionResult Cadastrar(){
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Cadastrar([FromForm]CadastrarUsuarioViewModel UsuarioVM){
            
            if(ModelState.IsValid){

                var resultado = await _UserManager.CreateAsync(PopularUsuarioModel(UsuarioVM),UsuarioVM.Senha);
                if(resultado.Succeeded){
                    this.MostrarMensagem("Usuario cadastrado com sucesso!");
                    return RedirectToAction("Login");
                } else {
                    this.MostrarMensagem($"{resultado.Errors}");
                    return View(UsuarioVM);
                }
            } else {
                this.MostrarMensagem("Credenciais invalidas, verifique as credenciais...");
                return View(UsuarioVM);
            }

        }

        public UsuarioModel PopularUsuarioModel (CadastrarUsuarioViewModel UsuarioVM){
            var Usuario = new UsuarioModel();
            Usuario.UserName = UsuarioVM.Email;
            Usuario.Email = UsuarioVM.Email;
            Usuario.NormalizedEmail = UsuarioVM.Email.ToUpper().Trim();
            Usuario.CPF = UsuarioVM.CPF;
            Usuario.NomeCompleto = UsuarioVM.NomeCompleto;
            Usuario.Telefone = UsuarioVM.Telefone;
            Usuario.DataNascimento = UsuarioVM.DataNascimento;

            return Usuario;
        }


    }

}