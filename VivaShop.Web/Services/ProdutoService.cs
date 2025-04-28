using System.Net.Http.Json;
using System.Text.Json;
using VivaShop.Models.DTOs;

namespace VivaShop.Web.Services
{
    public class ProdutoService : IProdutoService
    {
        public HttpClient _httpClient;
        public ILogger<ProdutoService> _logger;

        public ProdutoService(HttpClient httpClient, ILogger<ProdutoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProdutoDto> GetItem(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/produtos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Se a resposta não tiver conteúdo (StatusCode NoContent), retorna o valor padrão
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProdutoDto);
                    }

                    var jsonString = await response.Content.ReadAsStringAsync();

                    var produtoDto = JsonSerializer.Deserialize<ProdutoDto>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignorar diferenças de maiúsculas e minúsculas nos nomes das propriedades
                    });

                    return produtoDto; 

                }
                else
                {
                    var mensagem = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao obter produto pelo id: {id} - {mensagem}");
                    throw new Exception($"Erro ao buscar produto. StatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar a requisição.", ex);
            }
        }

        public async Task<IEnumerable<ProdutoDto>> GetItens()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/produtos");

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                var produtoDto = JsonSerializer.Deserialize<IEnumerable<ProdutoDto>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignorar diferenças de maiúsculas e minúsculas nos nomes das propriedades
                });

                return produtoDto ?? Enumerable.Empty<ProdutoDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar produtos : api/produtos. ERRO:{ex.Message}");
                throw;
            }
        }
    }
}
