using Microsoft.AspNetCore.Authorization;
using Utility.Constants;

namespace PetHealthCareSystemAPI.Auth
{

    internal class ApiPolicyAuthorizeAttribute : AuthorizeAttribute
    {
        public string Key {
            get
            {
                return Policy.Substring(IdentityConstant.ApiPolicyPrefix.Length);
            }
            set {
                Policy = $"{IdentityConstant.ApiPolicyPrefix}{value}";
            } 
        }

        public string Role => Key;
        public string DisplayName { get; set; }
        public ApiPolicyAuthorizeAttribute(string Role)
        {
            Key = Role;
        }
    }
}
