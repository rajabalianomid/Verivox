using System;
using System.Collections.Generic;
using System.Text;
using Verivox.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Domain
{
    public partial class AdvanceCondition : BaseEntity
    {
        public int ProductId { get; set; }
        public int FoulmulaId { get; set; }

        public virtual Formula TheFormula { get; set; }
    }
}
