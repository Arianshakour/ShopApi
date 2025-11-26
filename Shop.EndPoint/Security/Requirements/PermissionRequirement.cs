using Microsoft.AspNetCore.Authorization;

namespace Shop.EndPoint.Security.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        //این کلاس یک Requirement ساده است که فقط نگه می‌دارد چه دسترسی‌ای لازم است.
        public string PermissionKey { get; }

        public PermissionRequirement(string permissionKey)
        {
            PermissionKey = permissionKey;
        }
    }
}
