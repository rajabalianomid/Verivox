using Verivox.Common;
using Verivox.Common.Domain;
using Verivox.Domain;
using Verivox.Domain.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verivox.Service
{
    public class ProductService : PluginManager<IIntegratedMethod<List<ProductResult>, ProductSearch>>, IProductService
    {
        public ProductService(IPluginService pluginService) : base(pluginService)
        {
        }

        public List<ProductResult> OfferProductsByConsumption(ProductSearch model)
        {
            List<ProductResult> result = new List<ProductResult>();
            base.LoadAllPlugins(ServiceEnums.ConditionGroup.Electric.ToString()).ToList().ForEach(f =>
            {
                var productResult = f.ProcessIntegrated(new ProductSearch
                {
                    Consumption = model.Consumption
                });
                result.AddRange(productResult);
            });
            return result.OrderBy(o => o.AnnualCosts).ToList();
        }
    }
}
