using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.TypeDtos
{
    public class TypeGetDto
    {
        public int Id { get; set; }
        public string ProductTypeName { get; set; } = null!;
    }
}
