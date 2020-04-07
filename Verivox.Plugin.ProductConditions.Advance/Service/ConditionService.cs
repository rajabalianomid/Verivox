using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verivox.Common;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Advance.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Service
{
    public class ConditionService : IConditionService
    {
        private IRepository<Condition> _conditionRepository;
        private IRepository<Product> _productRepository;
        public ConditionService(IRepository<Condition> conditionRepository, IRepository<Product> productRepository)
        {
            this._conditionRepository = conditionRepository;
            this._productRepository = productRepository;
        }

        public List<ProductResult> GetProduct(ProductSearch productSearch)
        {
            var conditions = _conditionRepository.Table.ToList();
            var productIds = conditions.Select(s => s.ProductId);
            var foundProductByConditions = _productRepository.Table.Where(w => productIds.Any(a => a == w.ProductTypeId)).ToList();
            return conditions.Join(foundProductByConditions, condition => condition.ProductId, product => product.Id, (condition, product) => new { condition, product }).Select(s => new ProductResult
            {
                Id = s.condition.ProductId,
                Name = s.product.Name,
                AnnualCosts = (s.product.BaseAmount * productSearch.Consumption / 100) + (s.condition.ConsumptionCost * productSearch.Month)
            }).ToList();
        }
    }
}
