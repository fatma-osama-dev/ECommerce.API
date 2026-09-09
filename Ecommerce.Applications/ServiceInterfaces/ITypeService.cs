using Ecommerce.Application.DTOs.TypeDtos;
using Ecommerce.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
    public interface ITypeService
    {
        Task<BaseResponse<TypeGetDto>> CreateTypeAsync(TypeSendDto dto);
        Task<BaseResponse<string>> DeleteTypeAsync(int typeId);

        Task<BaseResponse<TypeGetDto>>UpdateTypeByTypeIdAsync(int typeId, TypeSendDto dto);
        Task<BaseResponse<TypeGetDto>> GetTypeByTypeIdAsync(int typeId);

        Task<BaseResponse<IReadOnlyCollection<TypeGetDto>>> GetAllTypesAsync();
        
    }
}
