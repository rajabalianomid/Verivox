using System;
using System.Collections.Generic;
using System.Text;

namespace Verivox.Domain.Search
{
    public class ProductSearch
    {
        public ProductSearch() { Month = 12; }
        public int Consumption { get; set; }
        public int Month { get; set; }
    }
}
