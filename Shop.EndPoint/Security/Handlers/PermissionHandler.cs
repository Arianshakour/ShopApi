using Microsoft.AspNetCore.Authorization;
using Shop.EndPoint.Security.Requirements;

namespace Shop.EndPoint.Security.Handlers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        //بررسی می‌کنیم که آیا کاربر claim‌ای به نام "permission" دارد که مقدارش همان PermissionKey باشد.

        //اگر داشت، context.Succeed(requirement) را صدا می‌زنیم تا سیستم بداند اجازه دارد.
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userPermissions = context.User.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value);

            if (userPermissions.Contains(requirement.PermissionKey))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
