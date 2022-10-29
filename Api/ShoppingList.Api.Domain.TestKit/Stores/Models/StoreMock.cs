using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public class StoreMock : Mock<IStore>
{
    public StoreMock(MockBehavior behavior, IStore store) : base(behavior)
    {
    }

    public void SetupId(StoreId id)
    {
        Setup(m => m.Id).Returns(id);
    }

    public void SetupName(StoreName name)
    {
        Setup(m => m.Name).Returns(name);
    }

    public void SetupIsDeleted(bool isDeleted)
    {
        Setup(m => m.IsDeleted).Returns(isDeleted);
    }

    public void SetupSections(IReadOnlyCollection<ISection> sections)
    {
        Setup(m => m.Sections).Returns(sections);
    }

    public void SetupChangeName(StoreName name)
    {
        Setup(m => m.ChangeName(name));
    }

    public void SetupContainsSection(SectionId sectionId, bool returnValue)
    {
        Setup(m => m.ContainsSection(sectionId)).Returns(returnValue);
    }

    public void SetupUpdateSectionsAsync(IEnumerable<SectionUpdate> sectionUpdates,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService)
    {
        Setup(m => m.UpdateSectionsAsync(sectionUpdates, itemModificationService, shoppingListModificationService))
            .Returns(Task.CompletedTask);
    }

    public void VerifyChangeName(StoreName name, Func<Times> times)
    {
        Verify(m => m.ChangeName(name), times);
    }

    public void VerifyUpdateSectionsAsync(IEnumerable<SectionUpdate> sectionUpdates,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService, Func<Times> times)
    {
        Verify(m => m.UpdateSectionsAsync(sectionUpdates, itemModificationService, shoppingListModificationService),
            times);
    }
}