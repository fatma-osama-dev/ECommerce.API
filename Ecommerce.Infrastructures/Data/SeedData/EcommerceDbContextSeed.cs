using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.SeedData
{
    public class EcommerceDbContextSeed
    {
        public static async Task SeedAsync(EcommerceDbContext context)
        {
            try
            {

                var path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData");
                if (!context.ProductBrands.Any())
                {
                    var brandsData = await File.ReadAllTextAsync(Path.Combine(path, "brands.json"));
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands is not null)
                    {
                        context.ProductBrands.AddRange(brands);
                        await context.SaveChangesAsync();
                    }
                }
                if (!context.ProductTypes.Any())
                {
                    var typesData = await File.ReadAllTextAsync(Path.Combine(path, "types.json"));
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types != null)
                    {
                        context.ProductTypes.AddRange(types);
                        await context.SaveChangesAsync();
                    }
                }

                if (!context.Products.Any())
                {
                    var productsData = await File.ReadAllTextAsync(Path.Combine(path, "products.json"));
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products != null)
                    {
                        context.Products.AddRange(products);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"خطأ في الـ Seed Data: {ex.Message}", ex);
            }
        }
    }
}
