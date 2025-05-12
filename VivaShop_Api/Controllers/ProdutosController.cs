using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VivaShop.Models.DTOs;
using VivaShop_Api.Interface;

namespace VivaShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger<ProdutosController> _logger;


        public ProdutosController(IProdutoRepository produtoRepository, ILogger<ProdutosController> logger)
        {
            _produtoRepository = produtoRepository;
            this._logger = logger;
        }

        // Método para buscar todas as categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
        {
            try
            {
                var categorias = await _produtoRepository.ObterTodasCategorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                // Retorna o erro 500 com uma mensagem personalizada
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // Método para buscar um produto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
        {
            try
            {
                var produto = await _produtoRepository.ObterProdutoPorId(id);
                if (produto == null) return NotFound("Produto não localizado.");
                return Ok(produto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados.");
            }
        }

        // Método para buscar todos os produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
        {
            try
            {
                var produtos = await _produtoRepository.ObterTodosProdutos();
                return Ok(produtos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados.");
            }
        }

        [HttpGet]
        [Route("{id}/GetItensPorCategoria")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetItensPorCategoria(int id)
        {
            try
            {
                var produtos = await _produtoRepository.GetItensPorCategoria(id);

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
                _logger.LogError($"Erro: {ex.Message}");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetCategoria")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoria()
        {
            try
            {
                var categorias = await _produtoRepository.GetCategoria();

                if (categorias == null || !categorias.Any())
                {
                    return NoContent();
                }

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar categorias: {ex.Message}");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
