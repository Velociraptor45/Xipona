using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToDomain
{
    public class ShoppingListConverter : IToDomainConverter<Entities.ShoppingList, IShoppingList>
    {
        private readonly IShoppingListFactory shoppingListFactory;
        private readonly IShoppingListSectionFactory shoppingListSectionFactory;
        private readonly IToDomainConverter<ItemsOnList, IShoppingListItem> shoppingListItemConverter;

        public ShoppingListConverter(IShoppingListFactory shoppingListFactory,
            IShoppingListSectionFactory shoppingListSectionFactory,
            IToDomainConverter<ItemsOnList, IShoppingListItem> shoppingListItemConverter)
        {
            this.shoppingListFactory = shoppingListFactory;
            this.shoppingListSectionFactory = shoppingListSectionFactory;
            this.shoppingListItemConverter = shoppingListItemConverter;
        }

        public IShoppingList ToDomain(Entities.ShoppingList source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var itemMapsPerSection = source.ItemsOnList.GroupBy(
                map => map.SectionId.Value,
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
                var items = maps.Select(map => shoppingListItemConverter.ToDomain(map)).ToList();
                var sectionModel = CreateSection(sectionId, items);
                sectionModels.Add(sectionModel);
            }

            return shoppingListFactory.Create(
                new ShoppingListId(source.Id),
                new StoreId(source.StoreId),
                source.CompletionDate,
                sectionModels);
        }

        public IShoppingListSection CreateSection(int sectionId, IEnumerable<IShoppingListItem> items)
        {
            return shoppingListSectionFactory.Create(
                new SectionId(sectionId),
                items);
        }
    }
}