using ApiPrincipal.Data;
using ApiPrincipal.Extensions;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Controllers
{

    public class PedidoController : Controller
    {
        private readonly IUnitOfWork _unit;

        private readonly UserManager<UsuarioModel> _user;

        private readonly DataContext _context;

        private readonly IBaseRepository<PedidoModel> _repository;
        public PedidoController(IUnitOfWork unit, IBaseRepository<PedidoModel> repository, UserManager<UsuarioModel> user, DataContext context)
        {
            _unit = unit;
            _repository = repository;
            _user = user;
            _context = context;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (this.HttpContext.User.IsInRole("administrador"))
            {
                var Pedidos = await _repository.BuscarTodos();
                return View(Pedidos);
            }
            else
            {
                var UsuarioAtivo = await _user.FindByNameAsync(this.HttpContext.User.Identity.Name);
                var Pedidos = await _context.Pedidos.Where(x => x.UserName == UsuarioAtivo.UserName).ToListAsync();
                return View(Pedidos);
            }
        }

        [Authorize]
        public async Task<ActionResult> Cadastrar()
        {

            await _repository.Cadastrar(new PedidoModel { UserName = this.HttpContext.User.Identity.Name, Situacao = "Aberto" });
            await _unit.Salvar();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> EditarPedido(int id){
            var ItensPedidos = await _context.ItensPedidos.AsNoTracking().Where(x => x.PedidoId == id).Include(x => x.Produto).ToListAsync();
            ViewBag.PedidoId = id;
            return View(ItensPedidos);
        }

        [Authorize]
        public async Task<ActionResult> FecharPedido(int id){
           var Pedido = await _repository.BuscarPorId(id);
           if(Pedido.ValorTotal <= 0){
            this.MostrarMensagem("Pedido não pode ser fechado com valor 0",TipoMensagem.Erro);
            return RedirectToAction("Index");
           }
           Pedido.DataFechamento = DateTime.Now ;
           Pedido.Situacao = "Fechado";
           await _unit.Salvar();
           return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EntregarPedido(int id){

            var Pedido = await _repository.BuscarPorId(id);

            if(Pedido.DataFechamento == null){
                this.MostrarMensagem("Não é possivel entregar um pedido sem ele estar fechado.",TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            var EnderecosBD = await _context.Enderecos.AsNoTracking().Where(x => x.UserName == Pedido.UserName).ToListAsync();

            if(EnderecosBD.Count == 0){
                this.MostrarMensagem("Nemhum endereço cadastrado na sua conta. Por favor cadastrar.",TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            List<SelectListItem> listaEnd = EnderecosBD.Select(x => new SelectListItem{
                Value = x.EnderecoId.ToString(),
                Text = $"Num - {x.EnderecoId}/ CEP - {x.cep} / Complemento - {x.complemento}"
        }).ToList();

        ViewBag.EnderecosEntregarPedido = listaEnd;

            return View(Pedido);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> EntregarPedido([FromForm]PedidoModel Model){
            
            if(Model.EnderecoId == 0){

                this.MostrarMensagem("Nemhum endereço selecionado.",TipoMensagem.Erro);
                
                return View(Model); 
            }

            var PedidoBD = await _repository.BuscarPorId(Model.PedidoId);

            PedidoBD.Situacao = "Saiu para entrega";
            await _unit.Salvar();

            this.MostrarMensagem($"Pedido numero {PedidoBD.PedidoId} Entregue!");
            return RedirectToAction("Index");

        }  

        [Authorize]
        public async Task<ActionResult> Excluir(int id)
        {

            var ItensPedidosDoPedido = await _context.ItensPedidos.AsNoTracking().Where(x => x.PedidoId == id).ToListAsync();

            foreach (var item in ItensPedidosDoPedido)
            {
                var ProdutoBD = await _context.Produtos.FindAsync(item.ProdutoId);
                ProdutoBD.QuantidadeEstoque += item.Quantidade ;
            }
             _context.ItensPedidos.RemoveRange(ItensPedidosDoPedido);
            await _repository.Excluir(id);
            await _unit.Salvar();
            return RedirectToAction("Index");
    }
    }
}


