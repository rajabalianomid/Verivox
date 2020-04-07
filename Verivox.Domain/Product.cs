using System;
using System.Collections.Generic;
using System.Text;

namespace Verivox.Domain
{
    public partial class Product: BaseEntity
    {
        public string Name { get; set; }
        public decimal BaseAmount { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
