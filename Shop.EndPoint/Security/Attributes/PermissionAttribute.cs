using Microsoft.AspNetCore.Authorization;

namespace Shop.EndPoint.Security.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        //Baraye sakhte Attribute balaye Action to controller lazeme in class
        public PermissionAttribute(string permissionKey)
        {
            Policy = permissionKey;
        }
    }
}
