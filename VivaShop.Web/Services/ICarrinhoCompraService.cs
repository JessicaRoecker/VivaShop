using VivaShop.Models.DTOs;

namespace VivaShop.Web.Services
{
    public interface ICarrinhoCompraService
    {
        Task<List<CarrinhoItemDto>> GetItems(string usuarioId);
        Task<CarrinhoItemDto> AdicionarItem(CarrinhoItemAdicionaDto carrinhoItemAdiciona);
        Task<CarrinhoItemDto> DeleteItem(int id);
        Task<CarrinhoItemDto> AtualizaQuantidade(CarrinhoItemAtualizaQuantidade carrinhoItemAtualiza);
       
        event Action<int> onCarrinhoCompraChanged;

        void RaiseEventoCarrinhoCompraChanged(int totalQuantidade);
    }
}
