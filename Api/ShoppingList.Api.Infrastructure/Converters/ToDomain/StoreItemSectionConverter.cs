using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class StoreItemSectionConverter : IToDomainConverter<Section, IStoreItemSection>
    {
        private readonly IStoreItemSectionFactory storeItemSectionFactory;

        public StoreItemSectionConverter(IStoreItemSectionFactory storeItemSectionFactory)
        {
            this.storeItemSectionFactory = storeItemSectionFactory;
        }

        public IStoreItemSection ToDomain(Section source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return storeItemSectionFactory.Create(
                new StoreItemSectionId(source.Id),
                source.Name,
                source.SortIndex);
        }
    }
}