using ApiPrincipal.Model;

namespace ApiPrincipal.Repositories.Interfaces{

    public interface IBaseRepository<T> where T : class{

        public Task<List<T>> BuscarTodos();

        public Task<T> BuscarPorId(int id);

        public void Atualizar(T model);

        public Task Cadastrar(T entidade);

        public Task Excluir(int id);

        public Task<CategoriaModel> BuscarCategoriaPorId(int id);

        public bool VerificarExistenciaPorNome(string NomeCategoria);
    }

}