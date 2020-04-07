using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verivox.Common;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Basic.Domain;

namespace Verivox.Plugin.ProductConditions.Basic.Service
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

        public async Task<List<ProductResult>> GetProduct(ProductSearch productSearch)
        {
            return await Task.Run(() =>
            {
                var conditions = _conditionRepository.Table.ToList();
                var productIds = conditions.Select(s => s.ProductId);
                var foundProductByConditions = _productRepository.Table.Where(w => productIds.Any(a => a == w.Id)).ToList();
                return conditions.Join(foundProductByConditions, condition => condition.ProductId, product => product.Id, (condition, product) => new { condition, product }).Select(s => new ProductResult
                {
                    Id = s.condition.ProductId,
                    Name = s.product.Name,
                    AnnualCosts = (s.product.BaseAmount * productSearch.Consumption / 100) + (s.condition.ConsumptionCost * productSearch.Month)
                }).ToList();
            });
        }
    }
}
