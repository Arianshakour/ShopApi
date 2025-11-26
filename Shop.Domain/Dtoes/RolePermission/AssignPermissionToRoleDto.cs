using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.RolePermission
{
    public class AssignPermissionToRoleDto
    {
        public int RoleId { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
