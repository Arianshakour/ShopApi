using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes
{
    //baraye inke pagination,sorting,filtering PUBLIC bashe bayad filter har entity joda tarif beshe
    //ba tavajo be field hash va az class haye sorting ers bari kone
    public class ProductFilterRequest : SortingRequest
    {
        public string? Search { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? CategoryId { get; set; }
    }

}
