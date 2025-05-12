using VivaShop.Models.DTOs;

namespace VivaShop_Api.Interface
{
    public interface IProdutoRepository
    {
        Task<ProdutoDto?> ObterProdutoPorId(int id);
        Task<IEnumerable<ProdutoDto>> ObterProdutosCarrinhoUsuario(IEnumerable<CarrinhoItemDto> carrinhoItens);
        Task<List<ProdutoDto>> ObterTodasCategorias();
        Task<List<ProdutoDto>> ObterTodosProdutos();
        Task<IEnumerable<ProdutoDto>> GetItensPorCategoria (int id);
        Task<IEnumerable<CategoriaDto>> GetCategoria();
    }
}