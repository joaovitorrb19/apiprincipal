using ApiPrincipal.Data;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using ApiPrincipal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Controllers {

    public class EnderecoController : Controller {
        private readonly IUnitOfWork _unit;

        private readonly IBaseRepository<EnderecoModel> _repository;

        private readonly UserManager<UsuarioModel> _user;

        private readonly DataContext _context;

        public EnderecoController(IUnitOfWork unit, IBaseRepository<EnderecoModel> repository, UserManager<UsuarioModel> user, DataContext context)
        {
            _unit = unit;
            _repository = repository;
            _user = user;
            _context = context;
        }

        [Authorize]
        public async Task<ActionResult> Index(){
            var UsuarioLogado = await _user.FindByNameAsync(this.HttpContext.User.Identity.Name);
            var Enderecos = await _context.Enderecos.AsNoTracking().Where(x => x.UserName == UsuarioLogado.UserName).ToListAsync();
            return View(Enderecos);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Cadastrar(int id){
            var UsuarioLogado = this.HttpContext.User.Identity.Name;
            if(id > 0){
                var EnderecoBD = await _repository.BuscarPorId(id);
                    if (UsuarioLogado == EnderecoBD.UserName){
                        return View(EnderecoBD);
                    } 
                this.MostrarMensagem("Você não possui autorização para alterar esse pedido",TipoMensagem.Erro);
                return RedirectToAction("Index");
                
            } else {
                return View();
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Cadastrar([FromForm]EnderecoModel Endereco){

            var UsuarioLogado = this.HttpContext.User.Identity.Name;

            var ListaEnderecosUsuaro = await _context.Enderecos.AsNoTracking().Where(x => x.UserName == Endereco.UserName).ToListAsync();

            var res = await BuscarEnderecoPorCepService.BuscarEndereco(Endereco);

            res.UserName = Endereco.UserName;
            if(res == null){
                ModelState.AddModelError("CEP","Cep Inválido");
                return View(Endereco);
            }
            
            if(ListaEnderecosUsuaro.Any( x => x.cep == Endereco.cep) && !(ListaEnderecosUsuaro.Any( x => x.EnderecoId == Endereco.EnderecoId))){
                ModelState.AddModelError("CEP","CEP já cadastrado na sua conta...");
                return View(Endereco);
            }

            if(Endereco.EnderecoId > 0){
                 _repository.Atualizar(res);
                 await _unit.Salvar();
                 this.MostrarMensagem("Endereço atualizado com sucesso");
                 return RedirectToAction("Index");
            } else {
                await _repository.Cadastrar(res);
                await _unit.Salvar();
                this.MostrarMensagem("Endereço cadastrado com sucesso");
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Excluir(int id){
            await _repository.Excluir(id);
            await _unit.Salvar();
            return RedirectToAction("Index");
        }

    }

}