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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.OwnsOne(u => u.Address, a =>
            {
                a.WithOwner(); 

                
                a.Property(p => p.Street).IsRequired().HasMaxLength(150);
                a.Property(p => p.City).IsRequired().HasMaxLength(50);
            });

            builder.Property(a => a.DisplayName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
