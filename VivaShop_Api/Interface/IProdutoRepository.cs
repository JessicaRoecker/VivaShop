using VivaShop.Models.DTOs;

namespace VivaShop_Api.Interface
{
    public interface IProdutoRepository
    {
        Task<ProdutoDto?> ObterProdutoPorId(int id);
        Task<List<ProdutoDto>> ObterTodasCategorias();
        Task<List<ProdutoDto>> ObterTodosProdutos();
    }
}