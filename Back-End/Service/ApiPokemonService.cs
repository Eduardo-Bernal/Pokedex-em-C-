using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BackEndProjeto.Services
{
    public class ApiPokemonService
    {
        private readonly HttpClient _httpClient;

        public ApiPokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        }

        // Verifica se o nome existe
        public async Task<bool> NomeValidoAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            // isso aqui e pra caso a pessoa digitar o nome dqls pokemon 
            // cheio de hifen ai voce troca ele pelo espaco e funciona tipo o mr mime (Testei ja) 
            nome = nome.Trim().ToLower().Replace(" ", "-");

            try
            {
                var response = await _httpClient.GetAsync($"pokemon/{nome}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // BUSCA A IMAGEM e retorna a URL
        public async Task<string?> BuscarImagemAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return null;

            nome = nome.Trim().ToLower().Replace(" ", "-");

            try
            {
                var response = await _httpClient.GetAsync($"pokemon/{nome}");
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);

                // Sprite oficial estilo GameBoy - essa API AQ e muito zika slk
                string? imagem = obj["sprites"]?["front_default"]?.ToString();

                return imagem;
            }
            catch
            {
                return null;
            }
        }
    }
}
