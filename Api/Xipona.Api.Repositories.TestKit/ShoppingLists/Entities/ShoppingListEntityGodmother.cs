using AutoFixture;
using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public class ShoppingListEntityGodmother
{
    private readonly List<Item> _items = [];
    private readonly List<(Guid ItemId, ItemType itemType)> _itemTypes = [];

    public ShoppingListEntityGodmother For(params Item[] items)
    {
        if(items.Any(i => i.ItemTypes.Count != 0))
            throw new ArgumentException("Items cannot have ItemTypes");
        
        _items.AddRange(items);
        return this;
    }

    public ShoppingListEntityGodmother For(params (Guid ItemId, ItemType itemType)[] itemTypes)
    {
        _itemTypes.AddRange(itemTypes);
        return this;
    }
    
    public ShoppingListEntityBuilder GetFoundation()
    {
        if(_items.Count == 0 && _itemTypes.Count == 0)
            return new ShoppingListEntityBuilder();

        var storeIdHashes = _items.Select(i => i.AvailableAt.Select(av => av.StoreId).ToHashSet());
        var typeStoreIdHashes = _itemTypes.Select(it => it.itemType.AvailableAt.Select(av => av.StoreId).ToHashSet());
        // find storeId that is in all hashsets
        var commonStoreId = storeIdHashes
            .Union(typeStoreIdHashes)
            .Aggregate((h1, h2) => { h1.IntersectWith(h2); return h1; })
            .ToList();
        if(commonStoreId.Count == 0)
            throw new ArgumentException("Items and ItemTypes must have at least one common StoreId");
        
        var storeId = commonStoreId[0];
        
        var items = _items
            .Select(i =>
            {
                var av = i.AvailableAt.First(a => a.StoreId == storeId);
                return new ItemsOnListEntityBuilder().WithItemId(i.Id).WithoutItemTypeId()
                    .WithSectionId(av.DefaultSectionId).Create();
            })
            .ToList();
        var itemTypes = _itemTypes
            .Select(it =>
            {
                var av = it.itemType.AvailableAt.First(a => a.StoreId == storeId);
                return new ItemsOnListEntityBuilder().WithItemId(it.ItemId).WithItemTypeId(it.itemType.Id)
                    .WithSectionId(av.DefaultSectionId).Create();
            })
            .ToList();
        return new ShoppingListEntityBuilder()
            .WithItemsOnList(items.Union(itemTypes).ToArray())
            .WithStoreId(storeId);
    }
}