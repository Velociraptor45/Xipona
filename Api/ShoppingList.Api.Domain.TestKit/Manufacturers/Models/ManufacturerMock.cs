using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Models;

public class ManufacturerMock : Mock<IManufacturer>
{
    public ManufacturerMock(IManufacturer manufacturer)
    {
        SetupId(manufacturer.Id);
        SetupName(manufacturer.Name);
        SetupIsDeleted(manufacturer.IsDeleted);
    }

    public void SetupId(ManufacturerId returnValue)
    {
        Setup(m => m.Id).Returns(returnValue);
    }

    public void SetupName(ManufacturerName returnValue)
    {
        Setup(m => m.Name).Returns(returnValue);
    }

    public void SetupIsDeleted(bool returnValue)
    {
        Setup(m => m.IsDeleted).Returns(returnValue);
    }

    public void SetupModify(ManufacturerModification modification)
    {
        Setup(m => m.Modify(modification));
    }

    public void VerifyModify(ManufacturerModification modification, Func<Times> times)
    {
        Verify(m => m.Modify(modification), times);
    }
}