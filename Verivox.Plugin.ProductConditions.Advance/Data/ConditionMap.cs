using Verivox.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Verivox.Data;
using Verivox.Plugin.ProductConditions.Advance.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Data
{
    public partial class ConditionMap : EntityTypeConfiguration<Condition>
    {
        public override void Configure(EntityTypeBuilder<Condition> builder)
        {
            builder.ToTable(nameof(Condition));
            builder.HasKey(condition => condition.Id);
            builder.Property(condition => condition.Name).IsRequired().HasMaxLength(100);

            base.Configure(builder);
        }
    }
}
