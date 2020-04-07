using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verivox.Common;
using Verivox.Common.Plugins;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Advance.Data;
using Verivox.Plugin.ProductConditions.Advance.Domain;
using Verivox.Plugin.ProductConditions.Advance.Service;

namespace Verivox.Plugin.ProductConditions.Advance
{
    public class ConditionProcessor : BasePlugin, IIntegratedMethod<List<ProductResult>, ProductSearch>
    {
        private IConditionService _conditionService;
        private ConditionContext _objectContext;
        public ConditionProcessor(IConditionService conditionService, ConditionContext objectContext)
        {
            _conditionService = conditionService;
            _objectContext = objectContext;
        }
        public List<ProductResult> ProcessIntegrated(ProductSearch processRequest)
        {
            return _conditionService.GetProduct(processRequest);
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
