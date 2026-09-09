
using Ecommerce.Domain.RepositoryInterfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
  public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EcommerceDbContext _context;
        public GenericRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
           _context.Set<T>().Add(entity);
           
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.FindAsync<T>(id);
            if (entity == null){
                return false; 
            }
            _context.Remove(entity);
           
            return true;
        }



        public async Task<IReadOnlyCollection<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }


        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
           return await _context.FindAsync<T>(id);
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);

            return Task.FromResult(entity);
        }
    }
}
