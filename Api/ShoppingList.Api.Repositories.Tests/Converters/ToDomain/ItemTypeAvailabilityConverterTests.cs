using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

internal class ItemTypeAvailabilityConverterTests : ToDomainConverterTestBase<ItemTypeAvailableAt, IItemAvailability>
{
    protected override (ItemTypeAvailableAt, IItemAvailability) CreateTestObjects()
    {
        var destination = ItemAvailabilityMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    public static ItemTypeAvailableAt GetSource(IItemAvailability destination)
    {
        return new ItemTypeAvailableAt
        {
            StoreId = destination.StoreId.Value,
            Price = destination.Price.Value,
            DefaultSectionId = destination.DefaultSectionId.Value
        };
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemAvailabilityFactory).Assembly,
            typeof(IItemAvailabilityFactory));
    }
}