using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Validations;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Services;

public class ItemCategoryValidationServiceMock : Mock<IItemCategoryValidationService>
{
    public ItemCategoryValidationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void VerifyValidateAsyncOnce(ItemCategoryId itemCategoryId)
    {
        Verify(i => i.ValidateAsync(
                itemCategoryId),
            Times.Once);
    }

    public void SetupValidateAsync(ItemCategoryId itemCategoryId)
    {
        Setup(m => m.ValidateAsync(itemCategoryId))
            .Returns(Task.CompletedTask);
    }
}