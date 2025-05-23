using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Common
{
    public static class MySortExtension
    {
        //in extenstion methodi hast ke baraye dynamic kardan sort estefade mishe
        public static IQueryable<TEntity> OrderByCustom<TEntity>
            (this IQueryable<TEntity> items, string sortBy, string sortOrder)
        {
            var type = typeof(TEntity);
            var expression2 = Expression.Parameter(type, "t");
            var property = type.GetProperty(sortBy);
            var expression1 = Expression.
                MakeMemberAccess(expression2, property);
            var Lambda = Expression.Lambda(expression1, expression2);
            var result = Expression.Call(
                typeof(Queryable),
                sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
                new Type[] { type, property.PropertyType },
                items.Expression,
                Expression.Quote(Lambda));

            return items.Provider.CreateQuery<TEntity>(result);
        }
    }
}
