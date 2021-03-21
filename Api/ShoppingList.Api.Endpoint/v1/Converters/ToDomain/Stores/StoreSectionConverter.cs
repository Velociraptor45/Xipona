using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores
{
    public class StoreSectionConverter : IToDomainConverter<StoreSectionContract, IStoreSection>
    {
        private readonly IStoreSectionFactory storeSectionFactory;

        public StoreSectionConverter(IStoreSectionFactory storeSectionFactory)
        {
            this.storeSectionFactory = storeSectionFactory;
        }

        public IStoreSection ToDomain(StoreSectionContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return storeSectionFactory.Create(
                new StoreSectionId(source.Id),
                source.Name,
                source.SortingIndex,
                source.IsDefaultSection);
        }
    }
}