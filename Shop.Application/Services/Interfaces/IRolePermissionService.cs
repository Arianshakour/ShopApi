using Shop.Domain.Dtoes.Permission;
using Shop.Domain.Dtoes.Role;
using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Interfaces
{
    public interface IRolePermissionService
    {
        // ===================== Role =====================
        void CreateRole(CreateRoleDto dto);
        void UpdateRole(int roleId, CreateRoleDto dto);
        void DeleteRole(int roleId);
        List<RoleDto> GetAllRoles();
        RoleDto GetRoleById(int roleId);

        // ===================== Permission =====================
        void CreatePermission(CreatePermissionDto dto);
        void UpdatePermission(int id, CreatePermissionDto dto);
        void DeletePermission(int id);
        List<PermissionDto> GetAllPermissions();
        PermissionDto GetPermissionById(int id);

        // ===================== Role-Permission Mapping =====================
        bool AssignPermissionsToRole(int roleId, List<int> permissionIds);


        // ===================== Role-User Mapping =====================
        bool AssignRolesToUser(int userId, List<int> roleIds);
    }
}
