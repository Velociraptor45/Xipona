using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ShoppingList.Api.Domain.TestKit.Items.Services.Validation;

public class ValidatorMock : Mock<IValidator>
{
    public ValidatorMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValidateAsync(IEnumerable<IItemAvailability> availabilities)
    {
        Setup(m => m.ValidateAsync(availabilities))
            .Returns(Task.CompletedTask);
    }

    public void SetupValidateAsync(ItemCategoryId itemCategoryId)
    {
        Setup(m => m.ValidateAsync(itemCategoryId))
            .Returns(Task.CompletedTask);
    }

    public void SetupValidateAsync(ManufacturerId manufacturerId)
    {
        Setup(m => m.ValidateAsync(manufacturerId))
            .Returns(Task.CompletedTask);
    }

    public void VerifyValidateAsync(IEnumerable<IItemAvailability> availabilities, Func<Times> times)
    {
        Verify(m => m.ValidateAsync(availabilities), times);
    }

    public void VerifyValidateAsync(ItemCategoryId itemCategoryId, Func<Times> times)
    {
        Verify(m => m.ValidateAsync(itemCategoryId), times);
    }

    public void VerifyValidateAsync(ManufacturerId manufacturerId, Func<Times> times)
    {
        Verify(m => m.ValidateAsync(manufacturerId), times);
    }

    public void VerifyValidateAsyncNever_ItemCategoryId()
    {
        Verify(m => m.ValidateAsync(It.IsAny<ItemCategoryId>()), Times.Never);
    }

    public void VerifyValidateAsyncNever_ManufacturerId()
    {
        Verify(m => m.ValidateAsync(It.IsAny<ManufacturerId>()), Times.Never);
    }
}