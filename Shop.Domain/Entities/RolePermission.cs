using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities
{
    public class RolePermission
    {
        [Key]
        public int RPId { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }


        //relation
        public Role role { get; set; }
        public Permission permission { get; set; }
    }
}
