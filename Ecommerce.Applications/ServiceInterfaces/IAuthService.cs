using Ecommerce.Application.DTOs.AccountDtos;
using Ecommerce.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<BaseResponse<UserDto>> LoginAsync(LoginDto loginDto);
    }
}
