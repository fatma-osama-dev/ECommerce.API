using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Ecommerce.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }



        public async Task<bool> DeleteCustomerBasketByBasketIdAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetCustomerBasketByBasketIdAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data!);
        }

        public async Task<CustomerBasket?> UpdateCustomerBasketAsync(CustomerBasket? basket)
        {
            if (basket is not null)
            {
                var serializedBasket = JsonSerializer.Serialize(basket);
                var createdOrUpdated = await _database.StringSetAsync(basket.Id, serializedBasket, TimeSpan.FromDays(30));

                if (!createdOrUpdated) return null;

                return await GetCustomerBasketByBasketIdAsync(basket.Id);
            }
            else
            {
                return null;

            }
        }
    }
}

