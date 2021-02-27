using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters
{
    public class StoreConverter : IToDomainConverter<Entities.Store, IStore>
    {
        private readonly IStoreFactory storeFactory;
        private readonly IToDomainConverter<Entities.Section, IStoreSection> storeSectionConverter;

        public StoreConverter(IStoreFactory storeFactory,
            IToDomainConverter<Entities.Section, IStoreSection> storeSectionConverter)
        {
            this.storeFactory = storeFactory;
            this.storeSectionConverter = storeSectionConverter;
        }

        public IStore ToDomain(Entities.Store source)
        {
            List<IStoreSection> sections = storeSectionConverter.ToDomain(source.Sections).ToList();

            return storeFactory.Create(
                new StoreId(source.Id),
                source.Name,
                source.Deleted,
                sections);
        }
    }
}