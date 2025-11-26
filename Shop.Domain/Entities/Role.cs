using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
        public bool Dlt { get; set; } = false;

        public List<UserRole> userRoles { get; set; }
        public List<RolePermission> rolePermissions { get; set; }
    }
}
