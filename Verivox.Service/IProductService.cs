using Verivox.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Verivox.Common.Plugins;
using Verivox.Common;
using Verivox.Domain.Search;

namespace Verivox.Service
{
    public interface IProductService
    {
        List<ProductResult> OfferProductsByConsumption(ProductSearch model);
    }
}
