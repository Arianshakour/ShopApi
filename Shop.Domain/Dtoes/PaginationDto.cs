using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes
{
    public class PaginationDto
    {
        const int _maxPageSize = 100;
        int _pageSize = 50;

        public int PageId { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = Math.Min(_maxPageSize, value);
            }
        }
    }
}
