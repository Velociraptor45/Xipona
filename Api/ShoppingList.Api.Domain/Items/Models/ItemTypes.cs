using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public class ItemTypes : IEnumerable<IItemType>
{
    private readonly Dictionary<ItemTypeId, IItemType> _itemTypes;
    private readonly IItemTypeFactory _itemTypeFactory;

    public ItemTypes(IEnumerable<IItemType> itemTypes, IItemTypeFactory itemTypeFactory)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemTypes = itemTypes.ToDictionary(t => t.Id);
    }

    public async Task ModifyManyAsync(IEnumerable<ItemTypeModification> modifications, IValidator validator)
    {
        var modificationsList = modifications.ToList();

        var typesToModify = modificationsList.Where(s => s.Id.HasValue)
            .ToDictionary(modification => modification.Id!.Value);
        var typesToCreate = modificationsList.Where(s => !s.Id.HasValue);
        var typeIdsToDelete = _itemTypes.Keys.Where(id => !typesToModify.ContainsKey(id));
        var newTypes = typesToCreate
            .Select(type => _itemTypeFactory.CreateNew(type.Name, type.Availabilities))
            .ToList();

        foreach (var typeId in typeIdsToDelete)
        {
            Delete(typeId);
        }

        foreach (var type in typesToModify.Values)
        {
            await ModifyAsync(type, validator);
        }

        AddMany(newTypes);
    }

    public async Task<ItemTypes> UpdateAsync(IEnumerable<ItemTypeUpdate> itemTypeUpdates, IValidator validator)
    {
        var typeUpdates = itemTypeUpdates.ToArray();
        var newTypes = new List<IItemType>();
        foreach (var update in typeUpdates)
        {
            IItemType newType;
            if (_itemTypes.TryGetValue(update.OldId, out var existingType))
            {
                newType = await existingType.UpdateAsync(update, validator);
                newTypes.Add(newType);
                continue;
            }

            await validator.ValidateAsync(update.Availabilities);
            newType = _itemTypeFactory.CreateNew(update.Name, update.Availabilities);
            newTypes.Add(newType);
        }

        return new ItemTypes(newTypes, _itemTypeFactory);
    }

    public IEnumerator<IItemType> GetEnumerator()
    {
        return _itemTypes.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IReadOnlyCollection<IItemType> GetForStore(StoreId storeId)
    {
        return _itemTypes.Values
            .Where(t => t.Availabilities.Any(av => av.StoreId == storeId))
            .ToList()
            .AsReadOnly();
    }

    public bool ContainsId(ItemTypeId itemTypeId)
    {
        return _itemTypes.ContainsKey(itemTypeId);
    }

    public bool TryGetValue(ItemTypeId id, out IItemType? itemType)
    {
        return _itemTypes.TryGetValue(id, out itemType);
    }

    public bool TryGetWithPredecessor(ItemTypeId predecessorId, out IItemType? itemType)
    {
        itemType = _itemTypes.Values
            .SingleOrDefault(t => t.PredecessorId == predecessorId);

        return itemType != null;
    }

    private void Delete(ItemTypeId id)
    {
        if (!_itemTypes.TryGetValue(id, out var type))
            return;

        _itemTypes[id] = type.Delete();
    }

    private async Task ModifyAsync(ItemTypeModification modification, IValidator validator)
    {
        if (!modification.Id.HasValue)
            throw new ArgumentException("Id mustn't be null.");

        if (!_itemTypes.TryGetValue(modification.Id.Value, out var type))
        {
            throw new DomainException(new ItemTypeNotFoundReason(modification.Id.Value));
        }

        var modifiedType = await type.ModifyAsync(modification, validator);
        _itemTypes[modifiedType.Id] = modifiedType;
    }

    private void AddMany(IEnumerable<IItemType> types)
    {
        foreach (var type in types)
        {
            Add(type);
        }
    }

    private void Add(IItemType section)
    {
        _itemTypes.Add(section.Id, section);
    }

    public ItemTypes Update(StoreId storeId, ItemTypeId? itemTypeId, Price price)
    {
        return new ItemTypes(
            _itemTypes.Values.Select(t =>
                t.IsAvailableAt(storeId) && (itemTypeId is null || t.Id == itemTypeId.Value)
                    ? t.Update(storeId, price)
                    : t.Update()),
            _itemTypeFactory);
    }

    public void TransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId)
    {
        foreach (var itemType in _itemTypes.Values)
        {
            if (!itemType.IsAvailableAt(oldSectionId))
                continue;

            _itemTypes[itemType.Id] = itemType.TransferToDefaultSection(oldSectionId, newSectionId);
        }
    }
}