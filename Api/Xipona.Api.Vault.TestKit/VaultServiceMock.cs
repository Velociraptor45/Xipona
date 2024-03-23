using Moq;

namespace ProjectHermes.Xipona.Api.Vault.TestKit;

public class VaultServiceMock : Mock<IVaultService>
{
    public VaultServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupLoadCredentialsAsync((string UserName, string Password) returnValue)
    {
        Setup(m => m.LoadCredentialsAsync())
            .ReturnsAsync(returnValue);
    }
}