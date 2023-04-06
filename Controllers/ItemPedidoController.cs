using ApiPrincipal.Data;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Controllers {

    public class ItemPedidoController : Controller {
        private readonly IUnitOfWork _unit;

        private readonly IBaseRepository<ItemPedidoModel> _repository;

        private readonly DataContext _context;


        public ItemPedidoController(IUnitOfWork unit, IBaseRepository<ItemPedidoModel> repository, DataContext context)
        {
            _unit = unit;
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Cadastrar(int ItemPedidoId,int PedidoId){

                var PedidoBD = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.PedidoId == PedidoId);
                if(PedidoBD.DataFechamento != null){
                    this.MostrarMensagem("Não é possivel adicionar itens em um Pedido fechado.",TipoMensagem.Erro);
                    return RedirectToAction("EditarPedido","Pedido",new { id = PedidoBD.PedidoId});
                }

            var ProdutosBD = await _context.Produtos.AsNoTracking().Include(x => x.Categoria).ToListAsync();
            List<SelectListItem> select = ProdutosBD.Select(x => new SelectListItem{
                Text = $"{x.NomeProduto} - {x.PrecoProduto.ToString("C")} unidade - {x.QuantidadeEstoque} disponiveis",
                Value = x.IdProduto.ToString()
            }).ToList();
            ViewBag.ProdutosItensPedidos = select;
            if(ItemPedidoId > 0){

                var ItemPedidoBD = await _repository.BuscarPorId(ItemPedidoId);
                return View(ItemPedidoBD);

            } else {
                return View(new ItemPedidoModel{PedidoId = PedidoId});
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Cadastrar([FromForm]ItemPedidoModel ItemPedido){

            var ProdutoBD = await _context.Produtos.FindAsync(ItemPedido.ProdutoId);

            var PedidoBD = await _context.Pedidos.FindAsync(ItemPedido.PedidoId);

             if(PedidoBD.DataFechamento != null){
                    this.MostrarMensagem("Não é possivel adicionar itens em um Pedido fechado.",TipoMensagem.Erro);
                    return RedirectToAction("EditarPedido","Pedido",new { id = PedidoBD.PedidoId});
                }
                

            if(ProdutoBD.QuantidadeEstoque < ItemPedido.Quantidade){
                ModelState.AddModelError("Quantidade","Quantidade não disponivel , verifique a quantidade disponivel novamente");
                var ProdutosBD = await _context.Produtos.Include(x => x.Categoria).ToListAsync();
            List<SelectListItem> select = ProdutosBD.Select(x => new SelectListItem{
                Text = $"{x.NomeProduto} - {x.PrecoProduto.ToString("C")} unidade - {x.QuantidadeEstoque} disponiveis",
                Value = x.IdProduto.ToString()
            }).ToList();
            ViewBag.ProdutosItensPedidos = select;
                return View(ItemPedido);
            } 

            if(ItemPedido.Quantidade <= 0){
                ModelState.AddModelError("Quantidade","Quantidade necessária");
                var ProdutosBD = await _context.Produtos.Include(x => x.Categoria).ToListAsync();
            List<SelectListItem> select = ProdutosBD.Select(x => new SelectListItem{
                Text = $"{x.NomeProduto} - {x.PrecoProduto.ToString("C")} unidade - {x.QuantidadeEstoque} disponiveis",
                Value = x.IdProduto.ToString()
            }).ToList();
            ViewBag.ProdutosItensPedidos = select;
                return View(ItemPedido);
            } 

            if(ItemPedido.ItemPedidoId > 0){
               
                var ItemPedidoBDAntesDaAtt = await _context.ItensPedidos.AsNoTracking().FirstOrDefaultAsync( x=> x.ItemPedidoId == ItemPedido.ItemPedidoId);
                ProdutoBD.QuantidadeEstoque += ItemPedidoBDAntesDaAtt.Quantidade;

                if(ProdutoBD.QuantidadeEstoque < ItemPedido.Quantidade){
                    ModelState.AddModelError("Quantidade","Quantidade maior que a disponivel no estoque");
                    return View(ItemPedido);
                }

                PedidoBD.ValorTotal -= ItemPedidoBDAntesDaAtt.Quantidade * ProdutoBD.PrecoProduto;
                ProdutoBD.QuantidadeEstoque -= ItemPedido.Quantidade;
                PedidoBD.ValorTotal += ItemPedido.Quantidade * ProdutoBD.PrecoProduto;

                _repository.Atualizar(ItemPedido);
                await _unit.Salvar();

                this.MostrarMensagem("Item atualizado com sucesso!");

                return RedirectToAction("EditarPedido","Pedido",new { id = ItemPedido.PedidoId});

            } else {

                if(ProdutoBD.QuantidadeEstoque < ItemPedido.Quantidade){
                    ModelState.AddModelError("Quantidade","Quantidade maior que a disponivel no estoque");
                    return View(ItemPedido);
                }
                ProdutoBD.QuantidadeEstoque -= ItemPedido.Quantidade ;

                await _repository.Cadastrar(ItemPedido);

                PedidoBD.ValorTotal += ItemPedido.Quantidade * ProdutoBD.PrecoProduto ;

                await _unit.Salvar();
                this.MostrarMensagem("Item adicionado com sucesso!");
                return RedirectToAction("EditarPedido","Pedido",new { id = ItemPedido.PedidoId});
            }
        }
        
        [Authorize]
        public async Task<ActionResult> Excluir(int id){

            var ItemPedidoBD = await _context.ItensPedidos.AsNoTracking().FirstOrDefaultAsync(x => x.ItemPedidoId == id);

            var ProdutoBD = await _context.Produtos.FindAsync(ItemPedidoBD.ProdutoId);
            var PedidoBD = await _repository.BuscarPorId(ItemPedidoBD.PedidoId);

            ProdutoBD.QuantidadeEstoque += ItemPedidoBD.Quantidade;

            await _repository.Excluir(id);
            await _unit.Salvar();
            return RedirectToAction("EditarPedido","Pedido",new { id = PedidoBD.PedidoId});
        }
    }

}
