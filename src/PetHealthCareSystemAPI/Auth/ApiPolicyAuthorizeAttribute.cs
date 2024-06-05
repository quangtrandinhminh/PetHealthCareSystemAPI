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
        public string Group { get; set; }
        public string GroupDisplayName { get; set; }
        public string DisplayName { get; set; }
        public ApiPolicyAuthorizeAttribute(string key, string group, string groupDisplayName, string displayName)
        {
            Key = key;
            Group = group;
            GroupDisplayName = groupDisplayName;
            DisplayName = displayName;
        }
    }
}
