using VivaShop.Models.DTOs;

namespace VivaShop.Web.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDto>> GetItens();
        Task<ProdutoDto> GetItem(int id);
        Task<IEnumerable<CategoriaDto>> GetCategorias();
        Task<IEnumerable<ProdutoDto>> GetItensPorCategoria(int id);
    }
}
