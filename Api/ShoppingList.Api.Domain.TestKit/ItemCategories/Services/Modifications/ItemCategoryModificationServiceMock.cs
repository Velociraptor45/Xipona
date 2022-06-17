using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Services.Modifications;

public class ItemCategoryModificationServiceMock : Mock<IItemCategoryModificationService>
{
    public ItemCategoryModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupModifyAsync(ItemCategoryModification modification)
    {
        Setup(m => m.ModifyAsync(modification))
            .Returns(Task.CompletedTask);
    }

    public void VerifyModifyAsync(ItemCategoryModification modification, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification), times);
    }
}