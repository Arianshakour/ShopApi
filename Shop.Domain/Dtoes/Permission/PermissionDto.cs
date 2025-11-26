using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.Permission
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionTitle { get; set; }
        public int? ParentId { get; set; }

        //baraye namayesh derakhti farzandan
        public List<PermissionDto> Children { get; set; }
    }
}
