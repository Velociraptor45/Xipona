using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class ItemAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, ItemAvailability>
{
    public ItemAvailability ToDomain(ItemAvailabilityContract source)
    {
        return new ItemAvailability(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}