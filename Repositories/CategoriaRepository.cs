using ApiPrincipal.Data;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Repositories {
    public class CategoriaRepository : IBaseRepository<CategoriaModel>{
            private readonly DataContext _context;

        public CategoriaRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaModel>> BuscarTodos(){
            var CategoriasBD = await _context.Categorias.ToListAsync();
            return CategoriasBD;
        }

        public async Task<CategoriaModel> BuscarPorId(int id){
            var CategoriaBD = await BuscarCategoriaPorId(id);
            return CategoriaBD;
        }

        public void Atualizar(CategoriaModel categoria){
             _context.Categorias.Entry(categoria).State = EntityState.Modified;
        }

        public async Task Cadastrar(CategoriaModel categoria){
           await _context.Categorias.AddAsync(categoria);
        }

        public  async Task Excluir(int id){
          var CategoriaBD = await BuscarCategoriaPorId(id);
           _context.Categorias.Remove(CategoriaBD);
        }

        public async Task<CategoriaModel> BuscarCategoriaPorId(int id){

            return await _context.Categorias.FindAsync(id);
        }

        public bool VerificarExistenciaPorNome(string NomeCategoria){
            var Resultado = _context.Categorias.Any(x => x.NomeCategoria == NomeCategoria);
            return Resultado;
        }

    }
}