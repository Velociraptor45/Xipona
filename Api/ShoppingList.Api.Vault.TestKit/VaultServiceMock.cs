using Moq;
using ShoppingList.Api.Vault.Configs;

namespace ShoppingList.Api.Vault.TestKit;

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