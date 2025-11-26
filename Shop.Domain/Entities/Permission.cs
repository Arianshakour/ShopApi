using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PermissionTitle { get; set; }
        //in baraye ine ke zir shakhe dare ya na
        //masalan modiriat karbaran ba id=1 parentidish mishe null 
        //bad afzoodan karbar parent idish mishe 1
        //yani kasi ke asan modiriat karbaro nadare nemitoone afzoodan ra dashte bashe
        //va hamchenin kasi ke modiriate karbaro dare bayad tik afzoodano dashte bashe ta btoone add kone
        //pas bayad in table yek relation be khodesh bezane
        //az jense list ham mizanim ke yani chanta mitoone zir majmooe dashte bashe
        public int? ParentId { get; set; }

        public string PermissionKey { get; set; } // مثال: "User.Create", "Product.Delete" , baraye dastresi

        //relations

        [ForeignKey("ParentId")]
        public List<Permission> permissions { get; set; }
        //niaz be yek jadval vaset ham darim chon chand be chand hast ba jadval Role
        public List<RolePermission> rolePermissions { get; set; }
    }
}
