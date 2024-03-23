using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

public class ShoppingListConverter : IToDomainConverter<Entities.ShoppingList, IShoppingList>
{
    private readonly IShoppingListFactory _shoppingListFactory;
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IToDomainConverter<ItemsOnList, ShoppingListItem> _shoppingListItemConverter;

    public ShoppingListConverter(IShoppingListFactory shoppingListFactory,
        IShoppingListSectionFactory shoppingListSectionFactory,
        IToDomainConverter<ItemsOnList, ShoppingListItem> shoppingListItemConverter)
    {
        _shoppingListFactory = shoppingListFactory;
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _shoppingListItemConverter = shoppingListItemConverter;
    }

    public IShoppingList ToDomain(Entities.ShoppingList source)
    {
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

        var list = (AggregateRoot)_shoppingListFactory.Create(
            new ShoppingListId(source.Id),
            new StoreId(source.StoreId),
            source.CompletionDate,
            sectionModels,
            source.CreatedAt);

        list.EnrichWithRowVersion(source.RowVersion);
        return (list as IShoppingList)!;
    }

    public IShoppingListSection CreateSection(Guid sectionId, IEnumerable<ShoppingListItem> items)
    {
        return _shoppingListSectionFactory.Create(
            new SectionId(sectionId),
            items);
    }
}