using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Xipona.Frontend.WebApp.Auth;

public class ArrayClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount>
    where TAccount : RemoteUserAccount
{
    public ArrayClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
    {
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(TAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);
        var claimsIdentity = (ClaimsIdentity)user.Identity!;

        if (account == null)
            return user;

        foreach (var kvp in account.AdditionalProperties)
        {
            var value = kvp.Value;

            if (value is JsonElement jsonElement) 
                ProcessJsonElement(jsonElement, kvp.Key, claimsIdentity);
        }

        return user;
    }

    private void ProcessJsonElement(JsonElement jsonElement, string typeName, ClaimsIdentity claimsIdentity)
    {
        if (jsonElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var jsonProperty in jsonElement.EnumerateObject())
            {
                ProcessJsonElement(jsonProperty.Value, typeName + $".{jsonProperty.Name}", claimsIdentity);
            }
        }
        
        if (jsonElement.ValueKind == JsonValueKind.Array)
        {
            claimsIdentity.TryRemoveClaim(claimsIdentity.FindFirst(typeName));

            var claims = jsonElement.EnumerateArray().Select(x => new Claim(typeName, x.ToString()));

            claimsIdentity.AddClaims(claims);
        }
        
        if(jsonElement.ValueKind == JsonValueKind.String)
        {
            claimsIdentity.TryRemoveClaim(claimsIdentity.FindFirst(typeName));

            var value = jsonElement.GetString()!;

            claimsIdentity.AddClaim(new Claim(typeName, value));
        }
    }
}