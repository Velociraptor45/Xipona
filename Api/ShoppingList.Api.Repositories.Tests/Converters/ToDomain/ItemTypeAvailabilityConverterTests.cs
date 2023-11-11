using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

internal class ItemTypeAvailabilityConverterTests : ToDomainConverterTestBase<ItemTypeAvailableAt, ItemAvailability>
{
    protected override (ItemTypeAvailableAt, ItemAvailability) CreateTestObjects()
    {
        var destination = ItemAvailabilityMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    public static ItemTypeAvailableAt GetSource(ItemAvailability destination)
    {
        return new ItemTypeAvailableAt
        {
            StoreId = destination.StoreId,
            Price = destination.Price,
            DefaultSectionId = destination.DefaultSectionId
        };
    }

    protected override void SetupServiceCollection()
    {
    }
}