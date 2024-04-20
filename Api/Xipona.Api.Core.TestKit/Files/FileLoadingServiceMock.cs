using Moq;
using ProjectHermes.Xipona.Api.Core.Files;

namespace ProjectHermes.Xipona.Api.Core.TestKit.Files;

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