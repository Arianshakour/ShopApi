using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Permission;
using Shop.Domain.Dtoes.Role;
using Shop.Domain.Entities;
using Shop.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Implementation
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _repo;

        public RolePermissionService(IRolePermissionRepository repo)
        {
            _repo = repo;
        }
        // ===================== Role =====================

        public void CreateRole(CreateRoleDto dto)
        {
            var role = new Role
            {
                RoleTitle = dto.RoleTitle
            };
            _repo.createRole(role);
        }

        public void UpdateRole(int roleId, CreateRoleDto dto)
        {
            var role = _repo.getRole(roleId);
            if (role == null) throw new NullReferenceException();

            role.RoleTitle = dto.RoleTitle;
            _repo.updateRole(role);
        }

        public void DeleteRole(int roleId)
        {
            var role = _repo.getRole(roleId);
            if (role == null) throw new NullReferenceException();

            role.Dlt = true;
            _repo.updateRole(role);
        }

        public List<RoleDto> GetAllRoles()
        {
            var roles = _repo.getRoles();
            return roles.Select(role => new RoleDto
            {
                RoleId = role.RoleId,
                RoleTitle = role.RoleTitle,
                Permissions = role.rolePermissions?
                               .Select(rp => new PermissionDto
                               {
                                   PermissionId = rp.permission.PermissionId,
                                   PermissionTitle = rp.permission.PermissionTitle
                               }).ToList() ?? new List<PermissionDto>()
            }).ToList();
        }

        public RoleDto GetRoleById(int roleId)
        {
            var role = _repo.getRole(roleId);
            if (role == null) return null;

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleTitle = role.RoleTitle,
                Permissions = role.rolePermissions?
                               .Select(rp => new PermissionDto
                               {
                                   PermissionId = rp.permission.PermissionId,
                                   PermissionTitle = rp.permission.PermissionTitle
                               }).ToList() ?? new List<PermissionDto>()
            };
        }

        // ===================== Permission =====================

        public void CreatePermission(CreatePermissionDto dto)
        {
            var permission = new Permission
            {
                PermissionTitle = dto.PermissionTitle,
                ParentId = dto.ParentId == 0 ? null : dto.ParentId
            };
            _repo.createPermission(permission);
        }

        public void UpdatePermission(int id, CreatePermissionDto dto)
        {
            var permission = _repo.getPermission(id);
            if (permission == null) throw new NullReferenceException();

            permission.PermissionTitle = dto.PermissionTitle;
            permission.ParentId = dto.ParentId == 0 ? null : dto.ParentId;
            _repo.updatePermission(permission);
        }

        public void DeletePermission(int id)
        {
            var permission = _repo.getPermission(id);
            if (permission == null) throw new NullReferenceException();

            _repo.deletePermission(permission);
        }

        public List<PermissionDto> GetAllPermissions()
        {
            var permissions = _repo.getPermissions();

            // شروع ساخت درخت از ریشه‌ها (ParentId == null)
            return BuildTree(permissions, null);
        }

        public PermissionDto GetPermissionById(int id)
        {
            var all = _repo.getPermissions();

            var root = all.FirstOrDefault(p => p.PermissionId == id);
            if (root == null)
                return null;

            // از تابع کمکی استفاده می‌کنیم
            return BuildTree(all, root.ParentId)
                   .FirstOrDefault(p => p.PermissionId == id);
        }

        //in tabe baraye sakhte halate derakhti dar permission hast ke hamaro biare
        private List<PermissionDto> BuildTree(List<Permission> allPermissions, int? parentId)
        {
            return allPermissions
                .Where(p => p.ParentId == parentId)
                .Select(p => new PermissionDto
                {
                    PermissionId = p.PermissionId,
                    PermissionTitle = p.PermissionTitle,
                    ParentId = p.ParentId,
                    Children = BuildTree(allPermissions, p.PermissionId)
                })
                .ToList();
        }

        // ===================== Role-Permission Mapping =====================

        public bool AssignPermissionsToRole(int roleId, List<int> permissionIds)
        {
            var role = _repo.getRole(roleId);
            if (role == null) return false;

            if(role.rolePermissions != null)
                role.rolePermissions.Clear();// حذف RolePermission های قبلی

            //hatman dar repository getRole , permission ra include kon
            foreach (var permId in permissionIds)
            {
                role.rolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permId
                });
            }

            _repo.updateRole(role);
            return true;
        }

        // ===================== Role-User Mapping =====================

        public bool AssignRolesToUser(int userId, List<int> roleIds)
        {
            var user = _repo.getUser(userId);
            if (user == null)
                return false;

            if(user.userRoles != null)
                user.userRoles.Clear();// حذف نقش‌های قبلی

            // اضافه کردن نقش‌های جدید
            foreach (var roleId in roleIds)
            {
                user.userRoles.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }

            _repo.updateUser(user);
            return true;
        }

    }
}
