namespace ProjectHermes.Xipona.Api.WebApp.Auth;

public class AuthenticationOptions
{
    public bool Enabled { get; set; } = false;
    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string[] ValidTypes { get; set; } = [];
    public string NameClaimType { get; set; } = "given_name";
    public string RoleClaimType { get; set; } = "role";
    public string UserRoleName { get; set; } = "User";
    public string OidcUrl => $"{Authority}/.well-known/openid-configuration";
}