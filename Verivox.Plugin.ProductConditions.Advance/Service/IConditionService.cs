using System;
using System.Collections.Generic;
using System.Text;
using Verivox.Domain;
using Verivox.Domain.Search;

namespace Verivox.Plugin.ProductConditions.Advance.Service
{
    public interface IConditionService
    {
        List<ProductResult> GetProduct(ProductSearch productSearch);
    }
}
