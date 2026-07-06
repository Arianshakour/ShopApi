using Shop.Domain.Dtoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Cache
{
    public static class CacheKeys
    {
        public static string Product(int id)
            => $"Product:{id}";

        public static string Products(FilteringDto filter)
        {
            return $"Products:" +
                   $"Search={filter.Search ?? ""}:" +
                   $"Category={filter.CategoryId?.ToString() ?? "All"}:" +
                   $"Min={filter.MinPrice?.ToString() ?? "0"}:" +
                   $"Max={filter.MaxPrice?.ToString() ?? "Max"}:" +
                   $"Sort={filter.SortBy}:" +
                   $"Order={filter.SortOrder}:" +
                   $"Page={filter.PageId}:" +
                   $"Size={filter.PageSize}";
        }
    }
}
