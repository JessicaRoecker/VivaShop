using Microsoft.AspNetCore.Mvc;
using VivaShop.Models.DTOs;
using VivaShop.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using VivaShop_Api.Interface;

namespace VivaShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
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
    }
}
