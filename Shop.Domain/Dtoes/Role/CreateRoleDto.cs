using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.Role
{
    public class CreateRoleDto
    {
        [Required]
        public string RoleTitle { get; set; }
    }
}
