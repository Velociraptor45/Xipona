using ProjectHermes.ShoppingList.Api.Vault.Configs;

namespace ProjectHermes.ShoppingList.Api.Vault;

public interface IVaultService
{
    Task<ConnectionStrings> LoadConnectionStringsAsync();
}