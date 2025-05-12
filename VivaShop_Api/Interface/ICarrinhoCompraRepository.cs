using VivaShop.Models.DTOs;

namespace VivaShop_Api.Interface
{
    public interface ICarrinhoCompraRepository
    {
        Task<CarrinhoItemDto> AdicionarItem(CarrinhoItemAdicionaDto carrinhoItemAdicionaDto);

        Task<CarrinhoItemDto> AtualizaQuantidade(int id, CarrinhoItemAtualizaQuantidade carrinhoItemAtualizaQuantidade);

        Task<CarrinhoItemDto> DeletaItem(int id);
        Task<CarrinhoItemDto> GetItem(int id);

        Task<IEnumerable<CarrinhoItemDto>> GetItens(string usuarioId);
    }
}
