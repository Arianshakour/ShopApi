using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Permission;
using Shop.Domain.Dtoes.Role;
using Shop.Domain.Dtoes.RolePermission;
using Shop.Domain.Dtoes.RoleUser;

namespace Shop.EndPoint.Controllers
{
    [ApiController]
    [Route("api/RolePermission")]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _service;

        public RolePermissionController(IRolePermissionService service)
        {
            _service = service;
        }

        // ===================== Role =====================

        [HttpPost("CreateRole")]
        public IActionResult CreateRole(CreateRoleDto dto)
        {
            _service.CreateRole(dto);
            return Ok();
        }

        [HttpPut("UpdateRole/{roleId}")]
        public IActionResult UpdateRole(int roleId, CreateRoleDto dto)
        {
            _service.UpdateRole(roleId, dto);
            return Ok();
        }

        [HttpDelete("DeleteRole/{roleId}")]
        public IActionResult DeleteRole(int roleId)
        {
            _service.DeleteRole(roleId);
            //return Ok(new { message = "Role deleted successfully" });
            return Ok();
        }

        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _service.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("GetRoleById/{roleId}")]
        public IActionResult GetRoleById(int roleId)
        {
            var role = _service.GetRoleById(roleId);
            if (role == null) return NotFound();
            return Ok(role);
        }

        // ===================== Permission =====================

        [HttpPost("CreatePermission")]
        public IActionResult CreatePermission(CreatePermissionDto dto)
        {
            _service.CreatePermission(dto);
            return Ok();
        }

        [HttpPut("UpdatePermission/{id}")]
        public IActionResult UpdatePermission(int id,CreatePermissionDto dto)
        {
            _service.UpdatePermission(id, dto);
            return Ok();
        }

        [HttpDelete("DeletePermission/{id}")]
        public IActionResult DeletePermission(int id)
        {
            _service.DeletePermission(id);
            return Ok();
        }

        [HttpGet("GetAllPermissions")]
        public IActionResult GetAllPermissions()
        {
            var permissions = _service.GetAllPermissions();
            return Ok(permissions);
        }

        [HttpGet("GetPermissionById/{id}")]
        public IActionResult GetPermissionById(int id)
        {
            var permission = _service.GetPermissionById(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        // ===================== Role-Permission Mapping =====================

        [HttpPost("AssignPermissionsToRole")]
        public IActionResult AssignPermissionsToRole([FromBody] AssignPermissionToRoleDto dto)
        {
            var success = _service.AssignPermissionsToRole(dto.RoleId, dto.PermissionIds);
            if (!success) return NotFound();
            return Ok(new { message = "Permissions assigned successfully" });
        }

        // ===================== Role-User Mapping =====================

        [HttpPost("AssignUsersToRole")]
        public IActionResult AssignUsersToRole([FromBody] AssignRoleToUserDto dto)
        {
            var success = _service.AssignRolesToUser(dto.UserId, dto.RoleIds);
            if (!success) return NotFound();
            return Ok(new { message = "Roles assigned successfully" });
        }
    }
}
