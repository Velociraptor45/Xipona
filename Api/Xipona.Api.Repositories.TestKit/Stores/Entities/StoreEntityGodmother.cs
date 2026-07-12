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
        _itemTypeAvailableAts = null;
        
        if(_availableAts.Select(av => av.StoreId).Distinct().Count() > 1)
            throw new ArgumentException("All AvailableAt must have the same StoreId");
        return this;
    }

    public StoreEntityGodmother For(ItemTypeAvailableAt[] itemTypeAvailableAts)
    {
        _itemTypeAvailableAts = itemTypeAvailableAts;
        _availableAts = null;
        
        if(_itemTypeAvailableAts.Select(ita => ita.StoreId).Distinct().Count() > 1)
            throw new ArgumentException("All ItemTypeAvailableAt must have the same StoreId");
        return this;
    }

    public StoreEntityGodmother For(ShoppingList shoppingList)
    {
        _shoppingList = shoppingList;
        return this;
    }

    public StoreEntityBuilder GetFoundation()
    {
        if (_availableAts is not null)
        {
            return new StoreEntityBuilder()
                .WithId(_availableAts[0].StoreId)
                .WithSections(CreateSections(_availableAts.Select(av => av.DefaultSectionId)));
        }
        if (_itemTypeAvailableAts is not null)
        {
            return new StoreEntityBuilder()
                .WithId(_itemTypeAvailableAts[0].StoreId)
                .WithSections(CreateSections(_itemTypeAvailableAts.Select(ita => ita.DefaultSectionId)));
        }
        if (_shoppingList is not null)
        {
            return new StoreEntityBuilder()
                .WithId(_shoppingList.Id)
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