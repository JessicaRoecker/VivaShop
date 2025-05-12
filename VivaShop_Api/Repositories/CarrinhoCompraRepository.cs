using System.Data;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using VivaShop.Models.DTOs;
using VivaShop_Api.Interface;

namespace VivaShop_Api.Repositories
{
    public class CarrinhoCompraRepository : ICarrinhoCompraRepository
    {

        private readonly string? _conectionString;

        public CarrinhoCompraRepository(IConfiguration configuration)
        {
            _conectionString = configuration.GetConnectionString("MinhaConexao");
        }

        public async Task<CarrinhoItemDto> AdicionarItem(CarrinhoItemAdicionaDto dto)
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                // Verificar se o item já existe no carrinho
                var sqlExiste = @"
                                SELECT *
                                FROM CarrinhoItems
                                WHERE CarrinhoId = @CarrinhoId AND ProdutoId = @ProdutoId";

                var existe = await connection.QueryFirstOrDefaultAsync<int?>(sqlExiste, new
                {
                    CarrinhoId = dto.CarrinhoId,
                    ProdutoId = dto.ProdutoId
                });

                if (existe != null)
                {
                    throw new InvalidOperationException("O item já existe no carrinho.");
                }

                // Verificar se o produto existe
                var sqlProduto = @"
                                    SELECT Id
                                    FROM Produtos
                                    WHERE Id = @ProdutoId";

                var produto = await connection.QueryFirstOrDefaultAsync<int?>(sqlProduto, new { ProdutoId = dto.ProdutoId });

                if (produto == null)
                {
                    throw new KeyNotFoundException("Produto não encontrado.");
                }

                // Criar o novo item no carrinho
                var novoItem = new CarrinhoItemDto
                {
                    CarrinhoId = dto.CarrinhoId,
                    ProdutoId = dto.ProdutoId,
                    QuantidadeId = dto.QuantidadeId,

                };
                var sqlInsert = @"
                                INSERT INTO CarrinhoItems (CarrinhoId, ProdutoId, QuantidadeId)
                                VALUES (@CarrinhoId, @ProdutoId, @QuantidadeId);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

                var id = await connection.ExecuteScalarAsync<int>(sqlInsert, novoItem);
                novoItem.Id = id;
                return novoItem;
            }
        }


        public async Task<CarrinhoItemDto> AtualizaQuantidade(int id, CarrinhoItemAtualizaQuantidade carrinhoItemAtualizaQuantidade)
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                var querySelect = @"
                                    SELECT * 
                                    FROM CarrinhoItems 
                                    WHERE ProdutoId = @id";

                var carrinho = await connection.QueryFirstOrDefaultAsync<CarrinhoItemDto>(querySelect, new { Id = id });

                if (carrinho is not null)
                {
                    var queryUpdate = @"
                                        UPDATE CarrinhoItems
                                        SET QuantidadeId = @QuantidadeId
                                        WHERE ProdutoId = @id";

                    await connection.ExecuteAsync(queryUpdate, new
                    {
                        QuantidadeId = carrinhoItemAtualizaQuantidade.QuantidadeId,
                        Id = id
                    });

                    carrinho.QuantidadeId = carrinhoItemAtualizaQuantidade.QuantidadeId;
                    return carrinho;
                }

                throw null;
            }
        }


        public async Task<CarrinhoItemDto> DeletaItem(int id)
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                var querySelect = @"
                                SELECT * 
                                FROM CarrinhoItems 
                                WHERE ProdutoId = @id";

                var item = await connection.QueryFirstOrDefaultAsync<CarrinhoItemDto>(querySelect, new { id });

                if (item == null)
                {
                    return null; 
                }

                var queryDelete = @"
                                    DELETE FROM CarrinhoItems 
                                    WHERE ProdutoId = @id";

                await connection.ExecuteAsync(queryDelete, new { id });

                return item;
            }
        }



        public async Task<CarrinhoItemDto> GetItem(int id)
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                var query = @"
                                SELECT 
                                    c.Id AS CarrinhoId,   
                                    ci.Id AS CarrinhoItem,
                                    ci.CarrinhoId,
                                    ci.QuantidadeId
                                FROM Carrinhos c
                                INNER JOIN CarrinhoItems ci ON c.Id = ci.CarrinhoId
                                WHERE ci.Id = @id";

                var results = await connection.QueryAsync<CarrinhoItemDto>(query, new { id });
                return results.FirstOrDefault() ?? new CarrinhoItemDto();
            }
        }


        public async Task<IEnumerable<CarrinhoItemDto>> GetItens(string usuarioId)
        {
            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                var query = @"
                                SELECT 
                                    c.Id ,   
                                    ci.Id ,
                                    ci.CarrinhoId,
                                    ci.QuantidadeId,
                                    ci.ProdutoId
                                FROM Carrinhos c
                                INNER JOIN CarrinhoItems ci ON c.Id = ci.CarrinhoId
                                WHERE c.UsuarioId = @usuarioId";

                var results = await connection.QueryAsync<CarrinhoItemDto>(query, new { usuarioId });
                return results;
            }
        }

       
    }
}
