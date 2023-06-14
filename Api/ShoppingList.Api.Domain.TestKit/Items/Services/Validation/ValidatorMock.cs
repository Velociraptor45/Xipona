using Moq.Language.Flow;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;

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

    public void SetupValidateAsync(IEnumerable<RecipeTagId> recipeTagIds)
    {
        Setup(m => m.ValidateAsync(recipeTagIds)).Returns(Task.CompletedTask);
    }

    public void SetupValidateAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        Setup(m => m.ValidateAsync(itemId, itemTypeId))
            .Returns(Task.CompletedTask);
    }

    public ISetup<IValidator, Task> SetupValidateAsyncAnd(ItemCategoryId itemCategoryId)
    {
        return Setup(m => m.ValidateAsync(itemCategoryId));
    }

    public ISetup<IValidator, Task> SetupValidateAsyncAnd(IEnumerable<IItemAvailability> availabilities)
    {
        return Setup(m => m.ValidateAsync(availabilities));
    }

    public ISetup<IValidator, Task> SetupValidateAsyncAnd(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return Setup(m => m.ValidateAsync(itemId, itemTypeId));
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

    public void VerifyValidateAsync(IEnumerable<RecipeTagId> recipeTagIds, Func<Times> times)
    {
        Verify(m => m.ValidateAsync(recipeTagIds), times);
    }

    public void VerifyValidateAsync(ItemId itemId, ItemTypeId? itemTypeId, Func<Times> times)
    {
        Verify(m => m.ValidateAsync(itemId, itemTypeId), times);
    }

    public void VerifyValidateAsyncNever_ItemCategoryId()
    {
        Verify(m => m.ValidateAsync(It.IsAny<ItemCategoryId>()), Times.Never);
    }

    public void VerifyValidateAsyncNever_ManufacturerId()
    {
        Verify(m => m.ValidateAsync(It.IsAny<ManufacturerId>()), Times.Never);
    }

    public void VerifyValidateAsyncNever_ItemId()
    {
        Verify(m => m.ValidateAsync(It.IsAny<ItemId>(), It.IsAny<ItemTypeId?>()), Times.Never);
    }
}