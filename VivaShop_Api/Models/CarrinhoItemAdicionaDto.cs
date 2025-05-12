using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaShop.Models.DTOs
{
    public class CarrinhoItemAdicionaDto
    {
        [Required]
        public int CarrinhoId { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        [Required]
        public int QuantidadeId { get; set; }

    }
}
