using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes
{
    public class PaginationRequest
    {
        //in class vorodi moshtarak Api ha hast baraye pagination
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        //100 gereftam ta client yeho 1000000 ta record nakhad
        public int PageSize { get; set; } = 10;
    }
}
