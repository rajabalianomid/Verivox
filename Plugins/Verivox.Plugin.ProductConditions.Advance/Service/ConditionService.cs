using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verivox.Common;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Plugin.ProductConditions.Advance.Domain;

namespace Verivox.Plugin.ProductConditions.Advance.Service
{
    public class ConditionService : IConditionService
    {
        private IRepository<AdvanceCondition> _conditionRepository;
        private IRepository<Product> _productRepository;
        public ConditionService(IRepository<AdvanceCondition> conditionRepository, IRepository<Product> productRepository)
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
