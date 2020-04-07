using Verivox.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Verivox.Data.Mapping
{
    public partial class ProductMap : EntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(product => product.Id);
            builder.Property(product => product.ProductTypeId).IsRequired();
            builder.HasOne(product => product.ProductType)
                .WithMany(ship => ship.Products)
                .HasForeignKey(foreignkey => foreignkey.ProductTypeId).OnDelete(DeleteBehavior.NoAction);

            base.Configure(builder);
        }
    }
}
