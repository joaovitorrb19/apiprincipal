using ApiPrincipal.Data;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Repositories {
    public class PedidoRepository : IBaseRepository<PedidoModel>{
            private readonly DataContext _context;

        public PedidoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<PedidoModel>> BuscarTodos(){
            var PedidosBD = await _context.Pedidos.ToListAsync();
            return PedidosBD;
        }

        public async Task<PedidoModel> BuscarPorId(int id){
            var PedidoBD = await BuscarEntidadePorId(id);
            return PedidoBD;
        }

        public void Atualizar(PedidoModel pedido){
             _context.Pedidos.Entry(pedido).State = EntityState.Modified;
        }

        public async Task Cadastrar(PedidoModel pedido){
           await _context.Pedidos.AddAsync(pedido);
        }

        public  async Task Excluir(int id){
          var PedidoBD = await BuscarEntidadePorId(id);
           _context.Pedidos.Remove(PedidoBD);
        }

        public async Task<PedidoModel> BuscarEntidadePorId(int id){

            return await _context.Pedidos.FindAsync(id);
        }

        public bool VerificarExistenciaPorNome(string NomePedido){
           // var Resultado = _context.Pedidos.Any(x => x.NomePedido == NomePedido);
            //return Resultado;
            throw new NotImplementedException();
        }

    }
}