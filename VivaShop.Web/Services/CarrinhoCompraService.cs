using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using VivaShop.Models.DTOs;

namespace VivaShop.Web.Services
{
    public class CarrinhoCompraService : ICarrinhoCompraService
    {
        private readonly HttpClient _httpClient;

        public CarrinhoCompraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public event Action<int> onCarrinhoCompraChanged;

        public async Task<CarrinhoItemDto> AdicionarItem(CarrinhoItemAdicionaDto carrinhoItemAdiciona)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<CarrinhoItemAdicionaDto>("api/CarrinhoCompra", carrinhoItemAdiciona);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CarrinhoItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<CarrinhoItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Messagem - {message}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CarrinhoItemDto> AtualizaQuantidade(CarrinhoItemAtualizaQuantidade carrinhoItemAtualiza)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(carrinhoItemAtualiza);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/CarrinhoCompra/{carrinhoItemAtualiza.ProdutoId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CarrinhoItemDto>();
                }
                return null;
            }
            catch
            {
                throw;
            }
           
        }

        public async Task<CarrinhoItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/CarrinhoCompra/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CarrinhoItemDto>();
                }
                return default(CarrinhoItemDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CarrinhoItemDto>> GetItems(string usuarioId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/CarrinhoCompra/{usuarioId}/GetItens");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CarrinhoItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CarrinhoItemDto>>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code: {response.StatusCode} Messagem: {message}");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void RaiseEventoCarrinhoCompraChanged(int totalQuantidade)
        {
            if(onCarrinhoCompraChanged != null)
            {
                onCarrinhoCompraChanged.Invoke(totalQuantidade);
            }
        }
    }
}
