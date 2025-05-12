using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaShop.Models.DTOs
{
   public class CarrinhoItemDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int CarrinhoId { get; set; }
    public int Quantidade { get; set; }
    public int QuantidadeId { get; set; }
    public string? Nome { get; set; } 
    public string? Descricao { get; set; } 
    public string? ImagemUrl { get; set; }
    public decimal PrecoTotal { get; set; }
    public decimal Preco { get; set; }
}

}
