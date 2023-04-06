using ApiPrincipal.Data;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Repositories {
    public class ProdutoRepository : IBaseRepository<ProdutoModel>{
            private readonly DataContext _context;

        public ProdutoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ProdutoModel>> BuscarTodos(){
            var ProdutosBD = await _context.Produtos.ToListAsync();
            return ProdutosBD;
        }

        public async Task<ProdutoModel> BuscarPorId(int id){
            var ProdutoBD = await BuscarEntidadePorId(id);
            return ProdutoBD;
        }

        public void Atualizar(ProdutoModel produto){
             _context.Produtos.Entry(produto).State = EntityState.Modified;
        }

        public async Task Cadastrar(ProdutoModel produto){
           await _context.Produtos.AddAsync(produto);
        }

        public  async Task Excluir(int id){
          var ProdutoBD = await BuscarEntidadePorId(id);
           _context.Produtos.Remove(ProdutoBD);
        }

        public async Task<ProdutoModel> BuscarEntidadePorId(int id){

            return await _context.Produtos.FindAsync(id);
        }

        public bool VerificarExistenciaPorNome(string NomeProduto){
            var Resultado = _context.Produtos.Any(x => x.NomeProduto == NomeProduto);
            return Resultado;
        }

    }
}