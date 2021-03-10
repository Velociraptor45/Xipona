using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class StoreItemStoreConverter : IToDomainConverter<Store, IStoreItemStore>
    {
        private readonly IStoreItemStoreFactory storeItemStoreFactory;
        private readonly IToDomainConverter<Section, IStoreItemSection> storeItemSectionConverter;

        public StoreItemStoreConverter(IStoreItemStoreFactory storeItemStoreFactory,
            IToDomainConverter<Section, IStoreItemSection> storeItemSectionConverter)
        {
            this.storeItemStoreFactory = storeItemStoreFactory;
            this.storeItemSectionConverter = storeItemSectionConverter;
        }

        public IStoreItemStore ToDomain(Store source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sectionEntities = source.Sections.ToList();

            List<IStoreItemSection> sections = storeItemSectionConverter.ToDomain(sectionEntities)
                .ToList();

            return storeItemStoreFactory.Create(
                new StoreItemStoreId(source.Id),
                source.Name,
                sections);
        }
    }
}