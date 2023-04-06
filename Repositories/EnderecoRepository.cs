using ApiPrincipal.Data;
using ApiPrincipal.Model;
using ApiPrincipal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiPrincipal.Repositories {
    public class EnderecoRepository : IBaseRepository<EnderecoModel>{
            private readonly DataContext _context;

        public EnderecoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<EnderecoModel>> BuscarTodos(){
            var EnderecosBD = await _context.Enderecos.ToListAsync();
            return EnderecosBD;
        }

        public async Task<EnderecoModel> BuscarPorId(int id){
            var EnderecoBD = await BuscarEntidadePorId(id);
            return EnderecoBD;
        }

        public void Atualizar(EnderecoModel endereco){
             _context.Enderecos.Entry(endereco).State = EntityState.Modified;
        }

        public async Task Cadastrar(EnderecoModel endereco){
           await _context.Enderecos.AddAsync(endereco);
        }

        public  async Task Excluir(int id){
          var EnderecoBD = await BuscarEntidadePorId(id);
           _context.Enderecos.Remove(EnderecoBD);
        }

        public async Task<EnderecoModel> BuscarEntidadePorId(int id){

            return await _context.Enderecos.FindAsync(id);
        }

        public bool VerificarExistenciaPorNome(string CEP){
            var Resultado = _context.Enderecos.Any(x => x.cep == CEP);
            return Resultado;
        }

    }
}