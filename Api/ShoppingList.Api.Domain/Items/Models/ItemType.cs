using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public class ItemType : IItemType
{
    public ItemType(ItemTypeId id, ItemTypeName name, IEnumerable<IItemAvailability> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();

        // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
        // https://github.com/AutoFixture/AutoFixture/issues/1108
        Predecessor = null;
    }

    public ItemTypeId Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<IItemAvailability> Availabilities { get; }
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

    public bool IsAvailableAt(StoreId storeId)
    {
        return Availabilities.Any(av => av.StoreId == storeId);
    }

    public bool IsAvailableAt(SectionId sectionId)
    {
        return Availabilities.Any(av => av.DefaultSectionId == sectionId);
    }

    public async Task<IItemType> ModifyAsync(ItemTypeModification modification, IValidator validator)
    {
        await validator.ValidateAsync(modification.Availabilities);

        return new ItemType(
            Id,
            modification.Name,
            modification.Availabilities);
    }

    public async Task<IItemType> UpdateAsync(ItemTypeUpdate update, IValidator validator)
    {
        await validator.ValidateAsync(update.Availabilities);

        var type = new ItemType(
            ItemTypeId.New,
            update.Name,
            update.Availabilities);
        type.SetPredecessor(this);

        return type;
    }

    public IItemType Update(StoreId storeId, Price price)
    {
        if (Availabilities.All(av => av.StoreId != storeId))
            throw new DomainException(new ItemTypeAtStoreNotAvailableReason(Id, storeId));

        var availabilities = Availabilities.Select(av =>
            av.StoreId == storeId
                ? new ItemAvailability(storeId, price, av.DefaultSectionId)
                : av);

        var newItemType = new ItemType(ItemTypeId.New, Name, availabilities);
        newItemType.SetPredecessor(this);
        return newItemType;
    }

    public IItemType Update()
    {
        var newItemType = new ItemType(
            ItemTypeId.New,
            Name,
            Availabilities);
        newItemType.SetPredecessor(this);

        return newItemType;
    }

    public IItemType TransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId)
    {
        if (!IsAvailableAt(oldSectionId))
            return this;

        var availabilities = new List<IItemAvailability>();
        for (int i = 0; i < Availabilities.Count; i++)
        {
            var availability = Availabilities.ElementAt(i);
            if (availability.DefaultSectionId == oldSectionId)
                availabilities.Add(availability.TransferToDefaultSection(newSectionId));
            else
                availabilities.Add(availability);
        }

        return new ItemType(
            Id,
            Name,
            availabilities);
    }
}