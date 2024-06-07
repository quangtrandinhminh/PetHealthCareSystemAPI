using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Utility.Constants;
using WaterCity.WebApi.Auth;

namespace PetHealthCareSystemAPI.Auth
{
    internal class ApiPolicyAuthorizationProvider : IAuthorizationPolicyProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DefaultAuthorizationPolicyProvider _defaultPolicyProvider { get; }

        public ApiPolicyAuthorizationProvider(IHttpContextAccessor httpContextAccessor, IOptions<AuthorizationOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _defaultPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(IdentityConstant.ApiPolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                var name = policyName.Substring(IdentityConstant.ApiPolicyPrefix.Length);
                policy.RequireAuthenticatedUser()
                      .AddRequirements(new ApiPolicyAuthorizationRequirement(name));
                return Task.FromResult(policy?.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return _defaultPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
