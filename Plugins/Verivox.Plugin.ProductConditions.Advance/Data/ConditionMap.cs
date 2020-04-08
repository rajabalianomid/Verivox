using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Verivox.Data;
using Verivox.Plugin.ProductConditions.Advance.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Data
{
    public partial class ConditionMap : EntityTypeConfiguration<AdvanceCondition>
    {
        public override void Configure(EntityTypeBuilder<AdvanceCondition> builder)
        {
            builder.ToTable(nameof(AdvanceCondition));
            builder.HasKey(condition => condition.Id);
            builder.HasOne(o => o.TheFormula).WithMany(m => m.AdvanceConditions).HasForeignKey(f => f.FoulmulaId);

            base.Configure(builder);
        }
    }
}
