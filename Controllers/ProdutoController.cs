using ApiPrincipal.Data;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Controllers {

    public class ProdutoController : Controller {
        private readonly IBaseRepository<ProdutoModel> _repository;
        private readonly IUnitOfWork _unit;
        public ProdutoController(IUnitOfWork unit, IBaseRepository<ProdutoModel> repository)
        {
            _unit = unit;
            _repository = repository;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Index([FromServices]DataContext context){
            var ProdutosBDD = await  context.Produtos.Include(x => x.Categoria).ToListAsync();
            return View(ProdutosBDD);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Cadastrar([FromServices]DataContext context,int id){
            
            var CategoriasBD = context.Categorias.ToList();
            List<SelectListItem> SelectListIte = CategoriasBD.Select(
                p => new SelectListItem{
                    Value = p.CategoriaId.ToString(),
                    Text  = p.NomeCategoria
                }
            ).ToList();
            ViewBag.Categorias = SelectListIte;

            if(id > 0){
                var UsuarioBD = await _repository.BuscarPorId(id);
                return View(UsuarioBD);
            } else {
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Cadastrar([FromForm]ProdutoModel Produto){
            if(_repository.VerificarExistenciaPorNome(Produto.NomeProduto)){
                ModelState.AddModelError("NomeProduto","Produto já cadastrado com esse nome");
                return View(Produto);
            }

             if((Produto.QuantidadeEstoque == 0 && Produto.QuantidadeEstoque < 0) && Produto.QuantidadeEstoque.ToString().Length > 3 ){
                ModelState.AddModelError("QuantidadeEstoque","Valor 0 ou mais que 3 digitos não permitido");
                return View(Produto);
            }

             if(Produto.PrecoProduto.ToString().Length == 0 && Produto.PrecoProduto.ToString().Length > 6){
                ModelState.AddModelError("PrecoProduto","Valor 0 ou mais que 6 digitos não permitido");
                return View(Produto);
            }


            if(Produto.IdProduto > 0){
                    Produto.DataAlteracao = DateTime.Now;
                    _repository.Atualizar(Produto);
                    await _unit.Salvar();
                    this.MostrarMensagem("Produto atualizado com sucesso!");
            } else {
                Produto.UsuarioQueCriou = this.HttpContext.User.Identity.Name;
               await _repository.Cadastrar(Produto);
               await _unit.Salvar();
               this.MostrarMensagem("Produto criado com sucesso!");
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> Excluir (int id){
           await _repository.Excluir(id);
           await _unit.Salvar();
           return RedirectToAction("Index");
        }
           
    }

}