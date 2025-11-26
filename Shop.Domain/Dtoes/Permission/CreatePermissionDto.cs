using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.Permission
{
    public class CreatePermissionDto
    {
        [Required]
        public string PermissionTitle { get; set; }

        public int? ParentId { get; set; }
    }
}
