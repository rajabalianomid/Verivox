using System.Collections.Generic;
using System.Linq;
using Verivox.Common;
using Verivox.Domain;
using Verivox.Domain.Search;

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
                List<ProductResult> productResult = f.ProcessIntegrated(new ProductSearch
                {
                    Consumption = model.Consumption
                });
                result.AddRange(productResult);
            });
            return result.OrderBy(o => o.AnnualCosts).ToList();
        }
    }
}
