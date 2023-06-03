using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Store : AggregateRoot, IStore
{
    private readonly Sections _sections;

    public Store(StoreId id, StoreName name, bool isDeleted, Sections sections)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        _sections = sections;
    }

    public StoreId Id { get; }
    public StoreName Name { get; private set; }
    public bool IsDeleted { get; }
    public IReadOnlyCollection<ISection> Sections => _sections.AsReadOnly();

    public ISection GetDefaultSection()
    {
        return _sections.GetDefaultSection();
    }

    public bool ContainsSection(SectionId sectionId)
    {
        return _sections.Contains(sectionId);
    }

    public void ChangeName(StoreName name)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedStoreReason(Id));

        Name = name;
    }

    public async Task ModifySectionsAsync(IEnumerable<SectionModification> sectionModifications,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedStoreReason(Id));

        await _sections.ModifyManyAsync(sectionModifications, itemModificationService, shoppingListModificationService);
    }
}