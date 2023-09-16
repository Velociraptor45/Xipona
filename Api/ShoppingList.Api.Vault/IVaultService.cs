namespace ProjectHermes.ShoppingList.Api.Vault;

public interface IVaultService
{
    Task<(string Username, string Password)> LoadCredentialsAsync();
}