using Microsoft.AspNetCore.Authorization;
using Utility.Constants;

namespace PetHealthCareSystemAPI.Auth
{
    internal class ApiPolicyAuthorizationHandler : AuthorizationHandler<ApiPolicyAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiPolicyAuthorizationRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == IdentityConstant.ApiClaimsType && x.Value == requirement.Key)) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}
