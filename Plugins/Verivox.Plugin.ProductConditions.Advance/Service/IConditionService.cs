using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Verivox.Domain;
using Verivox.Domain.Search;

namespace Verivox.Plugin.ProductConditions.Advance.Service
{
    public interface IConditionService
    {
        Task<List<ProductResult>> GetProduct(ProductSearch productSearch);
    }
}
