using System.Security.Claims;

namespace PetHealthCareSystemAPI.Extensions;

public static class ClaimExtension
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var stringId =  user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid")).Value;

        return int.Parse(stringId);
    }
}