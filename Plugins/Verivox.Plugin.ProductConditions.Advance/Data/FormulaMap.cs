using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verivox.Data;
using Verivox.Plugin.ProductConditions.Advance.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Data
{
    public class FormulaMap : EntityTypeConfiguration<Formula>
    {
        public override void Configure(EntityTypeBuilder<Formula> builder)
        {
            builder.ToTable(nameof(Formula));
            builder.HasKey(formula => formula.Id);
            builder.Property(formula => formula.Id).ValueGeneratedNever();

            base.Configure(builder);
        }
    }
}
