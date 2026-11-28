using Microsoft.Extensions.Configuration;

namespace Xipona.Api.Endpoint.Middleware;

public class AuthenticationOptions
{
    [ConfigurationKeyName("XIPONA_AUTH_ENABLED")]
    public bool Enabled { get; set; } = false;

    [ConfigurationKeyName("XIPONA_AUTH_AUTHORITY")]
    public string Authority { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_AUTH_AUDIENCE")]
    public string Audience { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_AUTH_VALID_TYPES")]
    public string[] ValidTypes { get; set; } = [];

    [ConfigurationKeyName("XIPONA_AUTH_CLAIM_NAME")]
    public string NameClaimType { get; set; } = "given_name";

    [ConfigurationKeyName("XIPONA_AUTH_CLAIM_ROLE")]
    public string RoleClaimType { get; set; } = "role";

    [ConfigurationKeyName("XIPONA_AUTH_ROLE_NAME_USER")]
    public string UserRoleName { get; set; } = "User";

    public string OidcUrl => $"{Authority}/.well-known/openid-configuration";
}