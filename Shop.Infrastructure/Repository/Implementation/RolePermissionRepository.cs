using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;
using Shop.Infrastructure.Context;
using Shop.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Repository.Implementation
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ShopContext _db;
        public RolePermissionRepository(ShopContext db)
        {
            _db = db;
        }

        public void createPermission(Permission permission)
        {
            _db.Permissions.Add(permission);
            _db.SaveChanges();
        }

        public void createRole(Role role)
        {
            _db.Roles.Add(role);
            _db.SaveChanges();
        }

        public void deletePermission(Permission permission)
        {
            _db.Permissions.Remove(permission);
            _db.SaveChanges();
        }

        public void deleteRole(Role role)
        {
            _db.Roles.Update(role);
            _db.SaveChanges();
        }

        public Permission getPermission(int id)
        {
            return _db.Permissions.FirstOrDefault(x => x.PermissionId == id);
        }

        public List<Permission> getPermissions()
        {
            return _db.Permissions.ToList();
        }

        public Role getRole(int id)
        {
            return _db.Roles.Include(r => r.rolePermissions).FirstOrDefault(x=>x.RoleId == id);
        }

        public List<Role> getRoles()
        {
            return _db.Roles.ToList();
        }

        public void updatePermission(Permission permission)
        {
            _db.Permissions.Update(permission);
            _db.SaveChanges();
        }

        public void updateRole(Role role)
        {
            _db.Roles.Update(role);
            _db.SaveChanges();
        }

        //User
        public User getUser(int id)
        {
            return _db.Users.Include(x => x.userRoles).FirstOrDefault(x => x.Id == id);
        }
        public void updateUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }
    }
}
