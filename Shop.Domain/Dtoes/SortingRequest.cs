using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes
{
    public class SortingRequest : PaginationRequest
    {
        public string? SortBy { get; set; } = "Id";

        private string _sortOrder = "asc";

        public string SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = string.Equals(
                    value, "desc", StringComparison.OrdinalIgnoreCase)
                    ? "desc"
                    : "asc";
            }
        }

    }
}
