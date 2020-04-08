using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verivox.Domain;

namespace Verivox.Data.Mapping
{
    public partial class ProductTypeMap : EntityTypeConfiguration<ProductType>
    {
        public override void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.ToTable(nameof(ProductType));
            builder.HasKey(producttype => producttype.Id);
            builder.Property(producttype => producttype.Id).ValueGeneratedNever();
            builder.Property(producttype => producttype.Name).IsRequired().HasMaxLength(100);

            base.Configure(builder);
        }
    }
}
