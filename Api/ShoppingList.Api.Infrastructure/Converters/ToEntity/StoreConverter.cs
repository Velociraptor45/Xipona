using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToEntity
{
    public class StoreConverter : IToEntityConverter<IStore, Entities.Store>
    {
        private readonly IToEntityConverter<IStoreSection, Section> sectionConverter;

        public StoreConverter(IToEntityConverter<IStoreSection, Section> sectionConverter)
        {
            this.sectionConverter = sectionConverter;
        }

        public Entities.Store ToEntity(IStore source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new Entities.Store()
            {
                Id = source.Id.Value,
                Name = source.Name,
                Deleted = source.IsDeleted,
                Sections = sectionConverter.ToEntity(source.Sections).ToList()
            };
        }
    }
}