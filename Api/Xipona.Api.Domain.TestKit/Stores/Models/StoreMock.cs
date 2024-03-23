using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;

public class StoreMock : Mock<IStore>
{
    public StoreMock(MockBehavior behavior, IStore store) : base(behavior)
    {
        SetupId(store.Id);
        SetupName(store.Name);
        SetupIsDeleted(store.IsDeleted);
        SetupSections(store.Sections);
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

    public void SetupModifySectionsAsync(IEnumerable<SectionModification> sectionUpdates,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService)
    {
        Setup(m => m.ModifySectionsAsync(sectionUpdates, itemModificationService, shoppingListModificationService))
            .Returns(Task.CompletedTask);
    }

    public void VerifyChangeName(StoreName name, Func<Times> times)
    {
        Verify(m => m.ChangeName(name), times);
    }

    public void VerifyModifySectionsAsync(IEnumerable<SectionModification> sectionUpdates,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService, Func<Times> times)
    {
        Verify(m => m.ModifySectionsAsync(sectionUpdates, itemModificationService, shoppingListModificationService),
            times);
    }

    public void SetupDelete()
    {
        Setup(m => m.Delete());
    }

    public void VerifyDelete(Func<Times> times)
    {
        Verify(m => m.Delete(), times);
    }
}