using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities
{
    public class UserRole
    {
        [Key]
        public int URId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public User user { get; set; }
        public Role role { get; set; }
    }
}
