using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.RoleUser
{
    public class AssignRoleToUserDto
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
