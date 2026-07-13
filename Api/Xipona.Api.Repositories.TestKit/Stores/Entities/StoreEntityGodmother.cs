using Xipona.Api.Repositories.Items.Entities;
using Xipona.Api.Repositories.ShoppingLists.Entities;
using Xipona.Api.Repositories.Stores.Entities;

namespace Xipona.Api.Repositories.TestKit.Stores.Entities;

public class StoreEntityGodmother
{
    private static readonly Random _rnd = new();
    private AvailableAt[]? _availableAts;
    private ItemTypeAvailableAt[]? _itemTypeAvailableAts;
    private ShoppingList? _shoppingList;

    public StoreEntityGodmother For(params AvailableAt[] availableAts)
    {
        _availableAts = availableAts;
        return this;
    }

    public StoreEntityGodmother For(params ItemTypeAvailableAt[] itemTypeAvailableAts)
    {
        _itemTypeAvailableAts = itemTypeAvailableAts;
        return this;
    }

    public StoreEntityGodmother For(ShoppingList shoppingList)
    {
        _shoppingList = shoppingList;
        return this;
    }

    public StoreEntityBuilder GetFoundation()
    {
        if (_availableAts is not null || _itemTypeAvailableAts is not null)
        {
            var storeIds = (_itemTypeAvailableAts?.Select(ita => ita.StoreId) ?? [])
                .Concat(_availableAts?.Select(av => av.StoreId) ?? [])
                .Distinct()
                .ToList();
            if(storeIds.Count > 1)
                throw new ArgumentException("All AvailableAt and ItemTypeAvailableAt must have the same StoreId");

            var sectionIds = (_availableAts?.Select(av => av.DefaultSectionId) ?? [])
                .Concat(_itemTypeAvailableAts?.Select(ita => ita.DefaultSectionId) ?? []);
            
            return new StoreEntityBuilder()
                .WithId(storeIds[0])
                .WithSections(CreateSections(sectionIds));
        }
        if (_shoppingList is not null)
        {
            return new StoreEntityBuilder()
                .WithId(_shoppingList.StoreId)
                .WithSections(CreateSections(_shoppingList.ItemsOnList.Select(i => i.SectionId)));
        }

        return new StoreEntityBuilder();
    }

    private static List<Section> CreateSections(IEnumerable<Guid> sectionIds)
    {
        var sections = new List<Section>();
        var sectionIdsList = sectionIds.ToList();
        var defaultSectionIdx = _rnd.Next(0, sectionIdsList.Count - 1);
        for (var i = 0; i < sectionIdsList.Count; i++)
        {
            var sectionId = sectionIdsList[i];
            var sectionBuilder = i == defaultSectionIdx
                ? SectionEntityMother.Default()
                : SectionEntityMother.NotDefault();
            
            var section = sectionBuilder
                .WithId(sectionId)
                .WithSortIndex(i)
                .Create();
            sections.Add(section);
        }

        return sections;
    }
}