using ApiPrincipal.Data;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers {
    public class CategoriaController : Controller {
        private readonly IBaseRepository<CategoriaModel> _repository;

        private readonly IUnitOfWork _unit;
        public CategoriaController(IBaseRepository<CategoriaModel> repository,IUnitOfWork unit)
        {
            _unit = unit;
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Cadastrar(int id = 0){
            
             if (id > 0){
                var CategoriaParaAtualizar = await _repository.BuscarPorId(id);
                return View(CategoriaParaAtualizar);
             } else {
                return View();
             }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Cadastrar([FromForm]CategoriaModel CategoriaFormulario){

        if(!_repository.VerificarExistenciaPorNome(CategoriaFormulario.NomeCategoria)){
            if(CategoriaFormulario.IdCategoria > 0){
                _repository.Atualizar(CategoriaFormulario);
                await _unit.Salvar();
                this.MostrarMensagem("Categoria atualizada com sucesso!");
            } else {
                await _repository.Cadastrar(CategoriaFormulario);
                await _unit.Salvar();
                this.MostrarMensagem("Categoria adicionada com sucesso!");
            }
        } else {
            this.MostrarMensagem("Categoria já Existe...",TipoMensagem.Erro);
        }
            return RedirectToAction("Index","Home");
        }

    }
}