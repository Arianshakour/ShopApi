using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.Category
{
    public class CategoryForEditDto
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }
    }
}
