using System.Linq;
using Verivox.Domain;

namespace Verivox.Service
{
    public static class PaginationExtension
    {
        public static IQueryable<TEntity> ToPaging<TEntity>(this IQueryable<TEntity> query, int index, int pageSize = 10) where TEntity : BaseEntity
        {
            pageSize = pageSize > 0 ? pageSize : 10;
            return query.OrderBy(o => o.Id).Skip(pageSize * index).Take(pageSize);
        }

        public static IQueryable<TEntity> ToPaging<TEntity>(this IOrderedQueryable<TEntity> query, int index, int pageSize = 10) where TEntity : BaseEntity
        {
            return query.Skip(pageSize * index).Take(pageSize);
        }
    }
}
