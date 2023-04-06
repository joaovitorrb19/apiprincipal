using ApiPrincipal.Data;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Repositories {
    public class ItemPedidoRepository : IBaseRepository<ItemPedidoModel>{
            private readonly DataContext _context;

        public ItemPedidoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ItemPedidoModel>> BuscarTodos(){
            var ItemPedidosBD = await _context.ItensPedidos.ToListAsync();
            return ItemPedidosBD;
        }

        public async Task<ItemPedidoModel> BuscarPorId(int id){
            var ItemPedidoBD = await BuscarEntidadePorId(id);
            return ItemPedidoBD;
        }

        public void Atualizar(ItemPedidoModel itempedido){
             _context.ItensPedidos.Entry(itempedido).State = EntityState.Modified;
        }

        public async Task Cadastrar(ItemPedidoModel itempedido){
           await _context.ItensPedidos.AddAsync(itempedido);
        }

        public  async Task Excluir(int id){
          var ItemPedidoBD = await BuscarEntidadePorId(id);
           _context.ItensPedidos.Remove(ItemPedidoBD);
        }

        public async Task<ItemPedidoModel> BuscarEntidadePorId(int id){

            return await _context.ItensPedidos.FindAsync(id);
        }

        public bool VerificarExistenciaPorNome(string CEP){
           // var Resultado = _context.ItensPedidos.Any(x => x.cep == CEP);
            return false;
        }

    }

}