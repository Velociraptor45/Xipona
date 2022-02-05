using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToDomain;

public class ShoppingListConverter : IToDomainConverter<Entities.ShoppingList, IShoppingList>
{
    private readonly IShoppingListFactory _shoppingListFactory;
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IToDomainConverter<ItemsOnList, IShoppingListItem> _shoppingListItemConverter;

    public ShoppingListConverter(IShoppingListFactory shoppingListFactory,
        IShoppingListSectionFactory shoppingListSectionFactory,
        IToDomainConverter<ItemsOnList, IShoppingListItem> shoppingListItemConverter)
    {
        _shoppingListFactory = shoppingListFactory;
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _shoppingListItemConverter = shoppingListItemConverter;
    }

    public IShoppingList ToDomain(Entities.ShoppingList source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        var itemMapsPerSection = source.ItemsOnList.GroupBy(
                map => map.SectionId,
                map => map,
                (sectionId, maps) => new
                {
                    SectionId = sectionId,
                    Maps = maps
                })
            .ToDictionary(t => t.SectionId, t => t.Maps);

        List<IShoppingListSection> sectionModels = new List<IShoppingListSection>();
        foreach (var sectionId in itemMapsPerSection.Keys)
        {
            var maps = itemMapsPerSection[sectionId];
            var items = maps.Select(map => _shoppingListItemConverter.ToDomain(map)).ToList();
            var sectionModel = CreateSection(sectionId, items);
            sectionModels.Add(sectionModel);
        }

        return _shoppingListFactory.Create(
            new ShoppingListId(source.Id),
            new StoreId(source.StoreId),
            source.CompletionDate,
            sectionModels);
    }

    public IShoppingListSection CreateSection(int sectionId, IEnumerable<IShoppingListItem> items)
    {
        return _shoppingListSectionFactory.Create(
            new SectionId(sectionId),
            items);
    }
}