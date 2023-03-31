using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Extensions {

    public enum TipoMensagem{
        Informacao,Erro
    }

    public class MensagemPartialModel {
        public MensagemPartialModel(string mensagem, TipoMensagem TipoMensagem)
        {
            this.TipoMensagem = TipoMensagem;
            this.Mensagem = mensagem;
        }
        public TipoMensagem TipoMensagem{get;set;}

        public string Mensagem{get;set;}


    }

    public static class MensagemPartialExtension {

        public static void MostrarMensagem(this Controller @this,string Mensagem,TipoMensagem TipoMensagem = TipoMensagem.Informacao){
                @this.TempData["Mensagem"] = Serializar(Mensagem,TipoMensagem);
        }

        public static string Serializar(string Mensagem,TipoMensagem TipoMensagem){
            var MensagemNaoSerializada = new MensagemPartialModel(Mensagem,TipoMensagem);
            var MensagemSerializada = JsonSerializer.Serialize<MensagemPartialModel>(MensagemNaoSerializada);
            return MensagemSerializada;
        }

        public static MensagemPartialModel Desserializar(string MensagemSerializada){
            var resultado = JsonSerializer.Deserialize<MensagemPartialModel>(MensagemSerializada);
            return resultado;
        }
        
    }
}