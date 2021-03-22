using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class StoreSectionConverter : IToDomainConverter<Entities.Section, IStoreSection>
    {
        private readonly IStoreSectionFactory storeSectionFactory;

        public StoreSectionConverter(IStoreSectionFactory storeSectionFactory)
        {
            this.storeSectionFactory = storeSectionFactory;
        }

        public IStoreSection ToDomain(Entities.Section source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return storeSectionFactory.Create(
                    new SectionId(source.Id),
                    source.Name,
                    source.SortIndex,
                    source.IsDefaultSection);
        }
    }
}