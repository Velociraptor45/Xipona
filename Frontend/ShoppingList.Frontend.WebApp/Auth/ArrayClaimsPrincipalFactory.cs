using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Auth;

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
            if (value is JsonElement { ValueKind: JsonValueKind.Array } element)
            {
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(kvp.Key));

                var claims = element.EnumerateArray().Select(x => new Claim(kvp.Key, x.ToString()));

                claimsIdentity.AddClaims(claims);
            }
        }

        return user;
    }
}