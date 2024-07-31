using Microsoft.AspNetCore.Authorization;

namespace PetHealthCareSystemAPI.Auth
{
    internal class ApiPolicyAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Key { get; set; }

        public ApiPolicyAuthorizationRequirement(string name)
        {
            Key = name;
        }
    }
}
