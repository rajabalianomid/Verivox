using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verivox.Common;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Advance.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Service
{
    public class ConditionService : IConditionService
    {
        private readonly IRepository<AdvanceCondition> _conditionRepository;
        private readonly IRepository<Product> _productRepository;
        public ConditionService(IRepository<AdvanceCondition> conditionRepository, IRepository<Product> productRepository)
        {
            _conditionRepository = conditionRepository;
            _productRepository = productRepository;
        }

        public async Task<List<ProductResult>> GetProduct(ProductSearch productSearch)
        {
            return await Task.Run(() =>
            {

                List<AdvanceCondition> conditions = _conditionRepository.Table.ToList();
                IEnumerable<int> productIds = conditions.Select(s => s.ProductId);
                List<Product> foundProductByConditions = _productRepository.Table.Where(w => productIds.Any(a => a == w.Id)).ToList();
                return conditions.Join(foundProductByConditions, condition => condition.ProductId, product => product.Id, (condition, product) => new { condition, product }).Select(s => new ProductResult
                {
                    Id = s.condition.ProductId,
                    Name = s.product.Name,
                    AnnualCosts = Calculate(new List<string>
                {
                    productSearch.Consumption.ToString(),
                    s.product.BaseAmount.ToString(),
                    productSearch.Month.ToString() },
                        s.condition.TheFormula.FormulaExpression)
                }).ToList();
            });
        }
        private decimal Calculate(IEnumerable<string> variables, string formulaExpression)
        {
            if (variables.Any())
            {
                formulaExpression = string.Format(formulaExpression, variables.ToArray());
            }
            Expression e = new Expression(formulaExpression);
            return (decimal)e.calculate();
        }
    }
}
