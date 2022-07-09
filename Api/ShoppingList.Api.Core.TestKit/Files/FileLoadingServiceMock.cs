using Moq;
using ProjectHermes.ShoppingList.Api.Core.Files;

namespace ProjectHermes.ShoppingList.Api.Core.TestKit.Files;

public class FileLoadingServiceMock : Mock<IFileLoadingService>
{
    public FileLoadingServiceMock(MockBehavior behavior) : base(behavior)
    { }

    public void SetupReadFile(string filePath, string returnValue)
    {
        Setup(m => m.ReadFile(filePath))
            .Returns(returnValue);
    }
}