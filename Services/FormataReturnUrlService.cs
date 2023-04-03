namespace ApiPrincipal.Services{
    public static class FormataReturnUrlService {

        public static string[] retornaUrlFormatada(string url){

            var resultado = url.Split("/");

            return resultado;
        } 

    }
}