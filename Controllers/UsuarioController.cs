using System.Text;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Services;
using ApiPrincipal.Services.Interfaces;
using ApiPrincipal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers
{

    public class UsuarioController : Controller
    {

        private readonly UserManager<UsuarioModel> _UserManager;

        private readonly SignInManager<UsuarioModel> _SignInManager;

        private readonly IEmailService _EmailService;

        public UsuarioController(UserManager<UsuarioModel> UserManager, SignInManager<UsuarioModel> signInManager, IEmailService emailService)
        {
            _UserManager = UserManager;

            _SignInManager = signInManager;
            _EmailService = emailService;
        }

        [HttpGet]
        public ActionResult Login(string? returnUrl)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (String.IsNullOrEmpty(returnUrl))
            {
                return View();
            }
            else
            {
                return View(new LoginViewModel { returnUrl = returnUrl });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromForm] LoginViewModel login)
        {

            if (ModelState.IsValid)
            {
                var resultado = await _SignInManager.PasswordSignInAsync(login.Email, login.Senha, login.LembrarDeMim, false);
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem("Bem vindo!");
                    if (String.IsNullOrEmpty(login.returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var ReturnUrlFormatada = FormataReturnUrlService.retornaUrlFormatada(login.returnUrl);
                        return RedirectToAction(ReturnUrlFormatada[2], ReturnUrlFormatada[1]);
                    }
                }
                else
                {
                    var UsuarioBD = await _UserManager.FindByEmailAsync(login.Email.ToUpper().Trim());
                    if (UsuarioBD != null && !_UserManager.IsEmailConfirmedAsync(UsuarioBD).Result)
                    {
                        ModelState.AddModelError("Email", "Email não confirmado");
                    }
                    else if (_UserManager.Users.Any(x => x.Email == login.Email))
                    {
                        ModelState.AddModelError("Senha", "Senha inválida");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "E-mail não cadastrado");
                    }
                    return View(login);
                }
            }
            else
            {

                this.MostrarMensagem("Credenciais inválidas, verifique os campos...", TipoMensagem.Erro);
                return View(login);
            }
        }

        [Authorize]
        public async Task<ActionResult> Logout()
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                await _SignInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                this.MostrarMensagem("Você não está logado...");
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Cadastrar([FromForm] CadastrarUsuarioViewModel UsuarioVM)
        {

            if (_UserManager.Users.Any(x => x.NormalizedEmail == UsuarioVM.Email.ToUpper().Trim()))
            {
                ModelState.AddModelError("Email", "E-mail já cadastrado");
            }
            if (_UserManager.Users.Any(x => x.CPF == UsuarioVM.CPF))
            {
                ModelState.AddModelError("CPF", "CPF já cadastrado");
            }

            if(!(UsuarioVM.CPF.Length == 11)){
                ModelState.AddModelError("CPF","Cpf inválido");
           }

            var VerificacaoNomeCompleto = UsuarioVM.NomeCompleto.Split(" ");
            if (VerificacaoNomeCompleto.Count() < 2 && VerificacaoNomeCompleto[1].Length < 2)
            {
                ModelState.AddModelError("NomeCompleto", "Nome Completo Invalido");
            }
            if (UsuarioVM.DataNascimento.Year <= 1900)
            {
                ModelState.AddModelError("DataNascimento", "Data de nascimento inválida");
            }

            if (ModelState.ErrorCount > 0)
            {
                return View(UsuarioVM);
            }
            else
            {
                try
                {
                    var resultado = await _UserManager.CreateAsync(PopularUsuarioModelService.PopularUsuarioModel(UsuarioVM), UsuarioVM.Senha);

                    var UsuarioBD = await _UserManager.FindByEmailAsync(UsuarioVM.Email.ToUpper().Trim());

                    var token = await _UserManager.GenerateEmailConfirmationTokenAsync(UsuarioBD);

                    var url = Url.Action(nameof(ConfirmacaoEmailRealizadoComSucesso), "Usuario", new { token, UsuarioBD.Email }, Request.Scheme);

                    var EmailParaEnviar = PopularEmailParaConfirmacaoDeEmailService.PopularEmailParaConfirmacaoDeEmail(UsuarioVM.Email, url);
                    var resultadopp = _EmailService.EnviarEmailAsync(EmailParaEnviar);

                    this.MostrarMensagem("Usuario cadastrado com sucesso! Por favor verifique o email");
                    return RedirectToAction("Login");
                } catch (Exception e){
                    this.MostrarMensagem(e.InnerException.Message,TipoMensagem.Erro);
                    return RedirectToAction("Login");
                }

            }
        }

        [HttpGet]
        public ActionResult CadastroRealizadoComSucesso()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmacaoEmailRealizadoComSucesso(string token, string email)
        {
            var usuarioBD = await _UserManager.FindByEmailAsync(email.ToUpper().Trim());
            await _UserManager.ConfirmEmailAsync(usuarioBD, token);
            return View();
        }

        [HttpGet]
        public ActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EsqueciSenha(EsqueciSenhaViewModel EsqueciSenha)
        {

            if (_UserManager.Users.Any(x => x.NormalizedEmail == EsqueciSenha.Email.ToUpper().Trim()))
            {
                var UsuarioBD = await _UserManager.FindByEmailAsync(EsqueciSenha.Email.ToUpper().Trim());
                var TokenResetSenha = await _UserManager.GeneratePasswordResetTokenAsync(UsuarioBD);
                var UrlResetSenha = Url.Action(nameof(RedefinirSenha), "Usuario", new { TokenResetSenha, EsqueciSenha.Email }, Request.Scheme);
                await _EmailService.EnviarEmailAsync(PopularEmailParaResetSenhaService.PopularEmailParaResetSenha(EsqueciSenha.Email, UrlResetSenha));
                this.MostrarMensagem("Email com o link de redefinição de senha foi enviado, por favor verifique seu e-mail...");
                return RedirectToAction("Login");
            }
            else
            {
                this.MostrarMensagem("E-mail informado nao cadastrado...", TipoMensagem.Erro);
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public ActionResult RedefinirSenha(string TokenResetSenha, string Email)
        {
            return View(new RedefinirSenhaViewModel { Email = Email, Token = TokenResetSenha });
        }

        [HttpPost]
        public async Task<ActionResult> RedefinirSenha([FromForm] RedefinirSenhaViewModel RedefSenha)
        {

            if (ModelState.IsValid)
            {
                var UsuarioBD = await _UserManager.FindByEmailAsync(RedefSenha.Email.ToUpper().Trim());
                var resultado = await _UserManager.ResetPasswordAsync(UsuarioBD, RedefSenha.Token, RedefSenha.NovaSenha);

                if (resultado.Succeeded)
                {
                    this.MostrarMensagem("Senha redefinida com sucesso!");
                    return RedirectToAction("Login");
                }
                else
                {
                    this.MostrarMensagem("Ocorreu um erro...", TipoMensagem.Erro);
                    return RedirectToAction("Login");
                }

            }
            else
            {
                this.MostrarMensagem("Credenciais inválidas...", TipoMensagem.Erro);
                return View(RedefSenha);
            }
        }


        [HttpGet]
        public ActionResult AcessoRestrito(string? returnUrl)
        {
            return View();
        }


    }

}