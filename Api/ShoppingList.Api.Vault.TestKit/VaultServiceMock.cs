using Moq;
using ProjectHermes.ShoppingList.Api.Vault.Configs;

namespace ProjectHermes.ShoppingList.Api.Vault.TestKit;

public class VaultServiceMock : Mock<IVaultService>
{
    public VaultServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupLoadConnectionStringsAsync(ConnectionStrings connectionStrings)
    {
        Setup(m => m.LoadConnectionStringsAsync())
            .ReturnsAsync(connectionStrings);
    }
}