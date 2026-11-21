using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AuthenticationOptions = Xipona.Api.Endpoint.Middleware.AuthenticationOptions;

public sealed class RoleClaimsTransformation(AuthenticationOptions authOptions) : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimSections = authOptions.RoleClaimType.Split(".");
        foreach (var identity in principal.Identities)
        {
            var sourceClaim = identity.FindFirst(claimSections[0]);
            if (sourceClaim is null)
                continue;

            var fullClaimTypeBuilder = new StringBuilder();
            fullClaimTypeBuilder.Append(claimSections[0]);

            JsonDocument document;
            try
            {
                document = JsonDocument.Parse(sourceClaim.Value);
            }
            catch (JsonException)
            {
                continue; // Not valid JSON
            }

            using (document)
            {
                var currentJsonElement = document.RootElement;

                // iterate until deepest-nested object is reached
                for (var i = 1; i < claimSections.Length; i++)
                {
                    var currentClaimSection = claimSections[i];
                    if (!currentJsonElement.TryGetProperty(currentClaimSection, out var child))
                        continue;

                    currentJsonElement = child;
                    fullClaimTypeBuilder.Append(".");
                    fullClaimTypeBuilder.Append(currentClaimSection);
                }

                // extract values into separate claims
                var fullClaimType = fullClaimTypeBuilder.ToString();
                if (currentJsonElement.ValueKind == JsonValueKind.String)
                {
                    AddClaim(identity, fullClaimType, currentJsonElement.GetString()!);
                    continue;
                }

                foreach (var role in currentJsonElement.EnumerateArray())
                {
                    if (role.ValueKind != JsonValueKind.String)
                        continue;

                    var value = role.GetString()!;

                    // Avoid adding duplicates if transformation runs again.
                    AddClaim(identity, fullClaimType, value);
                }
            }
        }

        return Task.FromResult(principal);
    }

    private static void AddClaim(ClaimsIdentity identity, string fullClaimType, string value)
    {
        if (!identity.HasClaim(c => c.Type == fullClaimType && c.Value == value))
            identity.AddClaim(new Claim(fullClaimType, value));
    }
}