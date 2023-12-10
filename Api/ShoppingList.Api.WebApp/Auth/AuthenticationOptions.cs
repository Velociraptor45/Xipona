namespace ProjectHermes.ShoppingList.Api.WebApp.Auth;

public class AuthenticationOptions
{
    public bool Enabled { get; set; } = false;
    public string Authority { get; set; } = "";
    public string Audience { get; set; } = "";
    public string[] ValidTypes { get; set; } = Array.Empty<string>();
    public string NameClaimType { get; set; } = "given_name";
    public string RoleClaimType { get; set; } = "role";
    public string UserRoleName { get; set; } = "User";
}