using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Services.Creations;

public class ManufacturerCreationServiceMock : Mock<IManufacturerCreationService>
{
    public ManufacturerCreationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreateAsync(ManufacturerName name, IManufacturer result)
    {
        Setup(m => m.CreateAsync(name)).ReturnsAsync(result);
    }

    public void VerifyCreateAsync(ManufacturerName name, Func<Times> times)
    {
        Verify(m => m.CreateAsync(name), times);
    }
}