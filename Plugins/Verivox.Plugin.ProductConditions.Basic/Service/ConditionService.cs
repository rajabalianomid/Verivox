using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verivox.Common;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Basic.Domain;

namespace Verivox.Plugin.ProductConditions.Basic.Service
{
    public class ConditionService : IConditionService
    {
        private readonly IRepository<Condition> _conditionRepository;
        private readonly IRepository<Product> _productRepository;
        public ConditionService(IRepository<Condition> conditionRepository, IRepository<Product> productRepository)
        {
            _conditionRepository = conditionRepository;
            _productRepository = productRepository;
        }

        public async Task<List<ProductResult>> GetProduct(ProductSearch productSearch)
        {
            return await Task.Run(() =>
            {
                List<Condition> conditions = _conditionRepository.Table.ToList();
                IEnumerable<int> productIds = conditions.Select(s => s.ProductId);
                List<Product> foundProductByConditions = _productRepository.Table.Where(w => productIds.Any(a => a == w.Id)).ToList();
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
