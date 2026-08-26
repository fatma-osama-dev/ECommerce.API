using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.BrandDtos
{
    public class BrandGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }=null!;
    }
}
