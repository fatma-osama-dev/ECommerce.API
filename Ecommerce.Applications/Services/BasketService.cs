using AutoMapper;
using Ecommerce.Application.DTOs.Basket;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        public BasketService(IBasketRepository basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }



        public async Task<BaseResponse<CustomerBasketDto>> GetCustomerBasketByBasketIdAsync(string basketId)
        {
            try
            {
                var basket = await _basketRepo.GetCustomerBasketByBasketIdAsync(basketId);

               
                if (basket == null)
                {
                    var emptyBasketDto = new CustomerBasketDto { id = basketId };
                    return new BaseResponse<CustomerBasketDto>(true, "Empty basket retrieved.", emptyBasketDto);
                }

                var result = _mapper.Map<CustomerBasketDto>(basket);
                return new BaseResponse<CustomerBasketDto>(true, "Customer basket retrieved successfully.", result);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CustomerBasketDto>(false, "An error occurred while retrieving the customer basket.", ex);
            }
        }

   
        public async Task<BaseResponse<CustomerBasketDto>> UpdateCustomerBasketAsync(CustomerBasketDto? basket)
        {
            try
            {
                if (basket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Basket data cannot be null.");
                }

             
                var basketEntity = _mapper.Map<CustomerBasket>(basket);

                var updatedBasket = await _basketRepo.UpdateCustomerBasketAsync(basketEntity);

            
                if (updatedBasket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "An error occurred while saving or updating the customer basket.");
                }

                return new BaseResponse<CustomerBasketDto>(true, "Customer basket updated successfully.", _mapper.Map<CustomerBasketDto>(updatedBasket));
            }
            catch (Exception ex)
            {
                return new BaseResponse<CustomerBasketDto>(false, "An error occurred while creating or updating the customer basket.", ex);
            }
        }

        public async Task<BaseResponse<bool>> DeleteCustomerBasketByBasketIdAsync(string basketId)
        {
            try
            {
                var isDeleted = await _basketRepo.DeleteCustomerBasketByBasketIdAsync(basketId);
                if (isDeleted)
                {
                    return new BaseResponse<bool>(true, "Customer basket deleted successfully.", true);
                }

                return new BaseResponse<bool>(false, "Customer basket not found or already deleted.", false);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, "An error occurred while deleting the customer basket.", ex);
            }
        }
    }
}
