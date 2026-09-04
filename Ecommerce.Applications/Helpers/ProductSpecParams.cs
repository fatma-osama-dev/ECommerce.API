using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Helpers
{
    public class ProductSpecParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        public string? Search { get; set; } = null!;
        public string? Sort { get; set; } = null!;
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 5;


    }
}
