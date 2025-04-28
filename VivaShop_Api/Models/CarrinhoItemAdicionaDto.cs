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
        public int CarrinhiId { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        [Required]
        public int Qusntidade { get; set; }

    }
}
