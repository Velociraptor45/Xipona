using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToEntity
{
    public class SectionConverter : IToEntityConverter<IStoreSection, Section>
    {
        public Section ToEntity(IStoreSection source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new Section
            {
                Id = source.Id.Value,
                Name = source.Name,
                SortIndex = source.SortingIndex,
                IsDefaultSection = source.IsDefaultSection
            };
        }
    }
}