using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VivaShop.Models.DTOs;
using VivaShop_Api.Interface;

namespace VivaShop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoCompraController : ControllerBase
    {
        private readonly ICarrinhoCompraRepository _carrinhoCompra;
        private readonly IProdutoRepository _produto;
        private readonly ILogger _logger;

        public CarrinhoCompraController(ICarrinhoCompraRepository carrinhoCompra, IProdutoRepository produto, ILogger<CarrinhoCompraController> logger)
        {
            _carrinhoCompra = carrinhoCompra;
            _produto = produto;
            this._logger = logger;
        }

        [HttpGet]
        [Route("{usuarioId}/GetItens")]
        public async Task<ActionResult<IEnumerable<CarrinhoItemDto>>> GetItens(string usuarioId)
        {
            try
            {
                var carrinhoItens = await _carrinhoCompra.GetItens(usuarioId);
                if (carrinhoItens == null || !carrinhoItens.Any())
                {
                    return NoContent(); // Retorna 204 se não houver itens
                }

                var produtos = await _produto.ObterProdutosCarrinhoUsuario(carrinhoItens);
                if (produtos == null || !produtos.Any())
                {
                    throw new Exception("Não existem produtos ....");
                }

                foreach (var produto in produtos)
                {
                    var carrinhoItem = carrinhoItens.FirstOrDefault(ci => ci.ProdutoId == produto.Id);
                    if (carrinhoItem.ProdutoId == produto.Id)
                    {
                        produto.QuantidadeId = carrinhoItem.QuantidadeId;
                    }
                }

                var jsonString = JsonSerializer.Serialize(produtos);

                var itens = JsonSerializer.Deserialize<List<CarrinhoItemDto>>(jsonString);
                if (itens == null || !itens.Any())
                {
                    return NoContent();
                }

                return Ok(itens);
            }
            catch (Exception ex)
            {
                // Log da exceção
                _logger.LogError($"Erro: {ex.Message}");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarrinhoItemDto>> GetItem(int id)
        {
            try
            {
                var carrinhoItem = await _carrinhoCompra.GetItem(id);
                if (carrinhoItem == null)
                {
                    return NotFound("Item não encontrado");
                }

                var produto = await _produto.ObterProdutoPorId(carrinhoItem.Id);
                if (produto == null)
                {
                    return NotFound("Item não existe na fonte de dados");
                }

                var jsonString = JsonSerializer.Serialize(produto);
                var itens = JsonSerializer.Deserialize<List<CarrinhoItemDto>>(jsonString);
                if (itens == null || !itens.Any())
                {
                    return NoContent();
                }

                return Ok(itens);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter item {id} do carrinho. {ex.Message}");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CarrinhoItemDto>> PosItem([FromBody] CarrinhoItemAdicionaDto carrinhoItemAdiciona)
        {
            try
            {
                var novoCarrinhoItem = await _carrinhoCompra.AdicionarItem(carrinhoItemAdiciona);
                if (novoCarrinhoItem == null)
                {
                    return NoContent();
                }

                var produto = await _produto.ObterProdutoPorId(novoCarrinhoItem.ProdutoId);
                if (produto == null)
                {
                    throw new Exception($"Produto não Localizado (Id:({carrinhoItemAdiciona.ProdutoId}))");
                }

                if (novoCarrinhoItem.ProdutoId == produto.Id)
                {
                    produto.QuantidadeId = novoCarrinhoItem.QuantidadeId;
                }

                var jsonString = JsonSerializer.Serialize(produto);
                var item = JsonSerializer.Deserialize<CarrinhoItemDto>(jsonString);

                if (item == null)
                {
                    return NoContent();
                }


                return CreatedAtAction(nameof(GetItem), new { id = novoCarrinhoItem.Id }, item);

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao adicionar um novo item no carrinho");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CarrinhoItemDto>> DeleteItem(int id)
        {
            try
            {
                var carrinhoItem = await _carrinhoCompra.DeletaItem(id);
                if (carrinhoItem == null)
                {
                    return NotFound();
                }

                var produto = await _produto.ObterProdutoPorId(carrinhoItem.ProdutoId);
                if (produto == null)
                {
                    return NotFound();
                }


                var jsonString = JsonSerializer.Serialize(produto);
                var item = JsonSerializer.Deserialize<CarrinhoItemDto>(jsonString);
                return Ok(item);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CarrinhoItemDto>> AtualizaQuantidade(int id, CarrinhoItemAtualizaQuantidade carrinhoItemAtualiza)
        {
            try
            {
                var carrinhoItem = await _carrinhoCompra.AtualizaQuantidade(id, carrinhoItemAtualiza);

                if (carrinhoItem == null)
                {
                    return NotFound();
                }

                var produto = await _produto.ObterProdutoPorId(id);

                if (carrinhoItem.ProdutoId == produto.Id)
                {
                    produto.QuantidadeId = carrinhoItem.QuantidadeId;
                }

                var jsonString = JsonSerializer.Serialize(produto);
                var item = JsonSerializer.Deserialize<CarrinhoItemDto>(jsonString);
                return Ok(item);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

    }
}
