using Shop.Domain.Dtoes.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.Role
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }

        // list dastresi haye role
        public List<PermissionDto> Permissions { get; set; }
    }
}
