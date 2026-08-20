using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> product)
        {
            product.HasOne(b => b.ProductBrand)
                  .WithMany(p => p.Products)
                  .HasForeignKey(b => b.BrandId).OnDelete(DeleteBehavior.SetNull);
            product.HasOne(t => t.ProductType)
                  .WithMany(p => p.Products)
                  .HasForeignKey(t => t.ProductTypeId).OnDelete(DeleteBehavior.SetNull);
            product.Property(p => p.Price).HasColumnType("decimal(18,2)");


        }
    }
}
