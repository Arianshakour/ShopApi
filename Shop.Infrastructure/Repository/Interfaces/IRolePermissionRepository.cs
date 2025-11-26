using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Repository.Interfaces
{
    public interface IRolePermissionRepository
    {
        void createRole(Role role);
        void updateRole(Role role);
        void deleteRole(Role role);
        Role getRole(int id);
        List<Role> getRoles();

        //Permission

        void createPermission(Permission permission);
        void updatePermission(Permission permission);
        void deletePermission(Permission permission);
        Permission getPermission(int id);
        List<Permission> getPermissions();

        //User
        User getUser(int id);
        void updateUser(User user);
    }
}
