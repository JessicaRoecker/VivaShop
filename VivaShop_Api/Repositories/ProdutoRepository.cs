namespace VivaShop_Api.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using VivaShop.Models.DTOs;
    using VivaShop_Api.Interface;

    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string? _conectionString;

        public ProdutoRepository(IConfiguration configuration)
        {
            _conectionString = configuration.GetConnectionString("MinhaConexao");
        }


        public async Task<List<ProdutoDto>> ObterTodasCategorias()
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                string query = @"
                SELECT 
                    p.Id,
                    p.Nome,
                    p.Descricao,
                    p.Preco,
                    p.CategoriaId,
                    c.Nome AS CategoriaNome
                FROM Produtos p
                INNER JOIN Categorias c ON p.CategoriaId = c.Id";

                var result = await connection.QueryAsync<ProdutoDto>(query);
                return result.ToList();
            }
        }


        // Método para buscar um item por ID
        public async Task<ProdutoDto?> ObterProdutoPorId(int id)
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                string query = @"
                SELECT p.*, c.Nome AS CategoriaNome 
                FROM Produtos p
                JOIN Categorias c ON p.CategoriaId = c.Id
                WHERE p.Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<ProdutoDto>(query, new { Id = id });
            }
        }


        // Método para buscar todos os produtos
        public async Task<List<ProdutoDto>> ObterTodosProdutos()
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                string query = @"
                SELECT p.*, c.Nome AS CategoriaNome 
                FROM Produtos p
                JOIN Categorias c ON p.CategoriaId = c.Id";

                var result = await connection.QueryAsync<ProdutoDto>(query);

                return result.ToList();
            }
        }

    }
}
