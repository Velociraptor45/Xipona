using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;

public class ItemTypeAvailabilityConverter : IToDomainConverter<ItemTypeAvailableAt, ItemAvailability>
{
    public ItemAvailability ToDomain(ItemTypeAvailableAt source)
    {
        return new ItemAvailability(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}