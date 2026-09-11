using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> order)
        {
            order.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
                a.Property(p => p.Street).IsRequired().HasMaxLength(150);
                a.Property(p => p.City).IsRequired().HasMaxLength(60);
            });

            order.HasMany(o => o.OrderItems)
                 .WithOne(oi => oi.Order)
                 .HasForeignKey(oi => oi.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);

            order.HasOne(o => o.DeliveryMethod)
                 .WithMany(d => d.Orders)
                 .HasForeignKey(o => o.DeliveryMethodId)
                 .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
