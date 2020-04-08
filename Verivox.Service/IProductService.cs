using System.Collections.Generic;
using Verivox.Domain;
using Verivox.Domain.Search;

namespace Verivox.Service
{
    public interface IProductService
    {
        List<ProductResult> OfferProductsByConsumption(ProductSearch model);
    }
}
