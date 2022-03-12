using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public class ItemTypes : IEnumerable<IItemType>
{
    private readonly Dictionary<ItemTypeId, IItemType> _itemTypes;
    private readonly IItemTypeFactory _itemTypeFactory;

    public ItemTypes(IEnumerable<IItemType> itemTypes, IItemTypeFactory itemTypeFactory)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemTypes = itemTypes.ToDictionary(t => t.Id);
    }

    public int Count => _itemTypes.Count;

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
            Remove(typeId);
        }

        foreach (var type in typesToModify.Values)
        {
            await ModifyAsync(type, validator);
        }

        AddMany(newTypes);
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

    public bool TryGetWithPredecessor(ItemTypeId predecessorId, out IItemType? predecessor)
    {
        predecessor = _itemTypes.Values
            .SingleOrDefault(t => t.Predecessor != null && t.Predecessor.Id == predecessorId);

        return predecessor != null;
    }

    private void Remove(ItemTypeId id)
    {
        _itemTypes.Remove(id);
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
}