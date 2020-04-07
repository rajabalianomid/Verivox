using System;
using System.Collections.Generic;
using System.Text;
using Verivox.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Domain
{
    public class Formula : BaseEntity
    {
        public string FormulaExpression { get; set; }

        public virtual ICollection<AdvanceCondition> AdvanceConditions { get; set; }
    }
}
