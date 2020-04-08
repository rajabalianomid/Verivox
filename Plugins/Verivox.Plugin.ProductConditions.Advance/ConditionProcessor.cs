using System.Collections.Generic;
using System.Threading.Tasks;
using Verivox.Common;
using Verivox.Common.Plugins;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Advance.Data;
using Verivox.Plugin.ProductConditions.Advance.Service;

namespace Verivox.Plugin.ProductConditions.Advance
{
    public class ConditionProcessor : BasePlugin, IIntegratedMethod<List<ProductResult>, ProductSearch>
    {
        private readonly IConditionService _conditionService;
        private readonly ConditionContext _objectContext;
        public ConditionProcessor(IConditionService conditionService, ConditionContext objectContext)
        {
            _conditionService = conditionService;
            _objectContext = objectContext;
        }
        public List<ProductResult> ProcessIntegrated(ProductSearch processRequest)
        {
            System.Runtime.CompilerServices.ConfiguredTaskAwaitable<List<ProductResult>> task = Task.Run(async () => await _conditionService.GetProduct(processRequest)).ConfigureAwait(false);
            return task.GetAwaiter().GetResult();
        }

        public override void Install()
        {
            _objectContext.Install();
        }

        public override void Uninstall()
        {
            _objectContext.Uninstall();
        }
    }
}
