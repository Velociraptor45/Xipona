using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public class ItemType : IItemType
{
    public ItemType(ItemTypeId id, string name, IEnumerable<IStoreItemAvailability> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();

        // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
        // https://github.com/AutoFixture/AutoFixture/issues/1108
        Predecessor = null;
    }

    public ItemTypeId Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
    public IItemType? Predecessor { get; private set; }

    public void SetPredecessor(IItemType predecessor)
    {
        Predecessor = predecessor;
    }

    public SectionId GetDefaultSectionIdForStore(StoreId storeId)
    {
        var availability = Availabilities.FirstOrDefault(av => av.StoreId == storeId);
        if (availability == null)
            throw new DomainException(new ItemTypeAtStoreNotAvailableReason(Id, storeId));

        return availability.DefaultSectionId;
    }

    public bool IsAvailableAtStore(StoreId storeId)
    {
        return Availabilities.Any(av => av.StoreId == storeId);
    }

    public async Task<IItemType> ModifyAsync(ItemTypeModification modification, IValidator validator)
    {
        await validator.ValidateAsync(modification.Availabilities);

        return new ItemType(
            Id,
            modification.Name,
            modification.Availabilities);
    }
}