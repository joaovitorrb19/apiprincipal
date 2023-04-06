using System.Text.Json;
using ApiPrincipal.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Services
{

    public static class BuscarEnderecoPorCepService
    {

        public async static Task<EnderecoModel> BuscarEndereco(EnderecoModel endereco)
        {
            HttpClient HttpClient = new HttpClient();
            var EnderecoRequest = await HttpClient.GetAsync($"https://viacep.com.br/ws/{endereco.cep}/json/");
            var JsonNaoSerializado = await EnderecoRequest.Content.ReadAsStringAsync();
            
             if(JsonNaoSerializado.Contains("erro")){
                return null;
             } else {
                
                EnderecoModel JsonDesserializado = JsonSerializer.Deserialize<EnderecoModel>(JsonNaoSerializado);
                JsonDesserializado.cep = JsonDesserializado.cep.Remove(5,1);

                if(endereco.complemento != null){
                    JsonDesserializado.complemento = endereco.complemento;
                }

                if(endereco.EnderecoId > 0){
                    JsonDesserializado.EnderecoId = endereco.EnderecoId;
                }
                return JsonDesserializado;
             }
        }  

    }

}