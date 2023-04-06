using ApiPrincipal.Data;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IBaseRepository<CategoriaModel> _repository;

        private readonly IUnitOfWork _unit;
        public CategoriaController(IBaseRepository<CategoriaModel> repository, IUnitOfWork unit)
        {
            _unit = unit;
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var CategoriasBD = await _repository.BuscarTodos();
            return View(CategoriasBD);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Cadastrar(int id = 0)
        {

            if (id > 0)
            {
                var CategoriaParaAtualizar = await _repository.BuscarPorId(id);
                return View(CategoriaParaAtualizar);
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Cadastrar([FromForm] CategoriaModel CategoriaFormulario, [FromServices] UserManager<UsuarioModel> UserM)
        {

            if (!_repository.VerificarExistenciaPorNome(CategoriaFormulario.NomeCategoria))
            {
                if (CategoriaFormulario.CategoriaId > 0)
                {
                    CategoriaFormulario.DataAlteracao = DateTime.Now;
                    _repository.Atualizar(CategoriaFormulario);
                    await _unit.Salvar();
                    this.MostrarMensagem("Categoria atualizada com sucesso!");
                }
                else
                {
                    CategoriaFormulario.UsuarioQueCriou = this.HttpContext.User.Identity.Name;
                    await _repository.Cadastrar(CategoriaFormulario);
                    await _unit.Salvar();
                    this.MostrarMensagem("Categoria adicionada com sucesso!");
                }
            }
            else
            {
                ModelState.AddModelError("NomeCategoria", "Categoria já cadastrada...");
            }
            return RedirectToAction("Index", "Categoria");
        }

        [Authorize]
        public async Task<ActionResult> Excluir(int id)
        {

            await _repository.Excluir(id);
            await _unit.Salvar();
            this.MostrarMensagem("Categoria Excluida com Sucesso!");
            return RedirectToAction("Index");
        }

    }
}