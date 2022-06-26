using ShoppingList.Api.Vault.Configs;

namespace ShoppingList.Api.Vault;

public interface IVaultService
{
    Task<ConnectionStrings> LoadConnectionStringsAsync();
}