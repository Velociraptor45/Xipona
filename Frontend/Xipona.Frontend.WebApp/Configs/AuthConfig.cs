using Microsoft.Extensions.Configuration;

namespace Xipona.Frontend.WebApp.Configs;

public sealed class AuthConfig
{
    [ConfigurationKeyName("XIPONA_AUTH_ENABLED")]
    public bool Enabled { get; init; } = false;

    [ConfigurationKeyName("XIPONA_AUTH_AUTHORITY")]
    public string Authority { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_AUTH_CLIENT_ID")]
    public string ClientId { get; set; } = string.Empty;

    [ConfigurationKeyName("XIPONA_AUTH_DEFAULT_SCOPES")]
    public string[] DefaultScopes { get; set; } = [];

    [ConfigurationKeyName("XIPONA_AUTH_RESPONSE_TYPE")]
    public string ResponseType { get; set; } = "code";

    [ConfigurationKeyName("XIPONA_AUTH_ROLE_NAME_USER")]
    public string UserRoleName { get; init; } = "User";

    [ConfigurationKeyName("XIPONA_AUTH_CLAIM_NAME")]
    public string NameClaimIdentifier { get; init; } = "given_name";

    [ConfigurationKeyName("XIPONA_AUTH_CLAIM_ROLE")]
    public string RoleClaimIdentifier { get; init; } = "role";

    [ConfigurationKeyName("XIPONA_AUTH_CLAIM_SCOPE")]
    public string ScopeClaimIdentifier { get; init; } = "scope";
}