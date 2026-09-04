using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.RepositoryInterfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetCustomerBasketByBasketIdAsync(string basketId);
        Task<CustomerBasket?> UpdateCustomerBasketAsync(CustomerBasket? basket);
        Task<bool> DeleteCustomerBasketByBasketIdAsync(string basketId);

    }
}
