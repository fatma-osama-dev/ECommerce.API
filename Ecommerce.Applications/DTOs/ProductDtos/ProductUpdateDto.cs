using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ProductDtos
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; } = null!;
        public string? Description { get; set; } = null!;

        public string? PictureUrl { get; set; } = null!;
        public decimal? Price { get; set; }
        public int? BrandId { get; set; }
        public int? ProductTypeId { get; set; }
    }
}
