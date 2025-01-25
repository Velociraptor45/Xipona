using Moq;
using ProjectHermes.Xipona.Api.Secrets.Vault;

namespace ProjectHermes.Xipona.Api.Secrets.TestKit;

public class VaultServiceMock : Mock<IVaultService>
{
    public VaultServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupLoadCredentialsAsync((string UserName, string Password) returnValue)
    {
        Setup(m => m.LoadDatabaseCredentialsAsync())
            .ReturnsAsync(returnValue);
    }
}