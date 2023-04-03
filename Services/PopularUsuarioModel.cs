using ApiPrincipal.Model;
using ApiPrincipal.ViewModel;

namespace ApiPrincipal.Services {
    public static class PopularUsuarioModelService{
        public static UsuarioModel PopularUsuarioModel(CadastrarUsuarioViewModel UsuarioVM)
        {
            var Usuario = new UsuarioModel();
            Usuario.UserName = UsuarioVM.Email;
            Usuario.Email = UsuarioVM.Email;
            Usuario.NormalizedEmail = UsuarioVM.Email.ToUpper().Trim();
            Usuario.CPF = UsuarioVM.CPF;
            Usuario.NomeCompleto = UsuarioVM.NomeCompleto;
            Usuario.Telefone = UsuarioVM.Telefone;
            Usuario.DataNascimento = UsuarioVM.DataNascimento;

            return Usuario;
        }

    }
}