using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes
{
    public class FilteringDto : PaginationDto
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }


        //in 2 ta baraye dynamic kardane sort gozashtam ke ydoone ham extention method ham ezafe kardam
        public string SortBy { get; set; } = "Id";
        //asc - desc default asc
        string _sortOrder = "asc";
        //SortOrder pishfarzesh asc hast va age har chizi bjoz desc vared kone ya asan hichi vared nakone mishe asc
        //dar qeyre in sorat mishe desc
        public string SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }
    }
}
