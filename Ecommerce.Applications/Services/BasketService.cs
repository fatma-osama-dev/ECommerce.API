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
                if (string.IsNullOrEmpty(basketId))
                {
                    basketId=Guid.NewGuid().ToString();
                    return new BaseResponse<CustomerBasketDto>(true, "New basket created.", new CustomerBasketDto { id = basketId });   
                }
                var basket = await _basketRepo.GetCustomerBasketByBasketIdAsync(basketId);
                if (basket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(
                        false,
                        "Basket not found.");
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
                if (basket.id == null)
                {
                    basket.id = Guid.NewGuid().ToString();

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
        public async Task<BaseResponse<CustomerBasketDto>> MergeOrAttachBasketAsync(string? userBasketId, string? anonymousBasketId)
        {
            try
            {
            
                if (string.IsNullOrEmpty(anonymousBasketId))
                {
                   
                    if (string.IsNullOrEmpty(userBasketId))
                    {
                        return new BaseResponse<CustomerBasketDto>(true, "User has no basket.");
                         
                    }

                  
                    var existingBasket = await _basketRepo.GetCustomerBasketByBasketIdAsync(userBasketId);
                 

                    if (existingBasket == null)
                    {
                        return new BaseResponse<CustomerBasketDto>(false, "User basket was not found.");
                       
                    }

                    return new BaseResponse<CustomerBasketDto>(true, "User basket retrieved successfully.", _mapper.Map<CustomerBasketDto>(existingBasket));
                 
                }


              
                
                var anonymousBasket = await _basketRepo.GetCustomerBasketByBasketIdAsync(anonymousBasketId);
              

                if (anonymousBasket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Anonymous basket was not found.");
                
                }


                if (string.IsNullOrEmpty(userBasketId))
                {
                    return new BaseResponse<CustomerBasketDto>(true, "Anonymous basket will be attached to user.", _mapper.Map<CustomerBasketDto>(anonymousBasket));
                 
                }


            
                if (userBasketId == anonymousBasketId)
                {
                    return new BaseResponse<CustomerBasketDto>(true, "Basket is already associated with user.", _mapper.Map<CustomerBasketDto>(anonymousBasket));
                }


                var userBasket = await _basketRepo.GetCustomerBasketByBasketIdAsync(userBasketId);
            

                if (userBasket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "User basket was not found.");
                   
                }


                foreach (var anonymousItem in anonymousBasket.Basket)
                {
                    var existingItem = userBasket.Basket.FirstOrDefault(x => x.Id == anonymousItem.Id);
                   

                    if (existingItem != null)
                    {
                        existingItem.Quantity += anonymousItem.Quantity;
                
                    }
                    else
                    {
                        userBasket.Basket.Add(anonymousItem);
                    }
                }


                
                var updatedBasket = await _basketRepo.UpdateCustomerBasketAsync(userBasket);

                if (updatedBasket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Failed to update user basket.");
                
                }


         
                await _basketRepo.DeleteCustomerBasketByBasketIdAsync(anonymousBasket.Id);
                   


                return new BaseResponse<CustomerBasketDto>(true, "Baskets merged successfully.", _mapper.Map<CustomerBasketDto>(updatedBasket));
          
            }
            catch (Exception ex)
            {
                return new BaseResponse<CustomerBasketDto>(false, "An error occurred while merging baskets.", ex);
              
            }
        }
    }
}
