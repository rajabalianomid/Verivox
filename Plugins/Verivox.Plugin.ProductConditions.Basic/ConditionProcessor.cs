﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verivox.Common;
using Verivox.Common.Plugins;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Basic.Data;
using Verivox.Plugin.ProductConditions.Basic.Domain;
using Verivox.Plugin.ProductConditions.Basic.Service;

namespace Verivox.Plugin.ProductConditions.Basic
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
            var task = Task.Run(async () => await _conditionService.GetProduct(processRequest)).ConfigureAwait(false);
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
