using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Basket
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public decimal Price { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}
