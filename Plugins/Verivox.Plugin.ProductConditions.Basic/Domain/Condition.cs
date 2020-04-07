using System;
using System.Collections.Generic;
using System.Text;
using Verivox.Domain;

namespace Verivox.Plugin.ProductConditions.Basic.Domain
{
    public partial class Condition : BaseEntity
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal ConsumptionCost { get; set; }
    }
}
