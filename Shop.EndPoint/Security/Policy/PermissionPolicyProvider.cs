using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Shop.EndPoint.Security.Requirements;

namespace Shop.EndPoint.Security.Policy
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        //"هر Policy با نام X، نیاز دارد که کاربر حق دسترسی X را داشته باشد."
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            return Task.FromResult(policy);
        }
    }
}
