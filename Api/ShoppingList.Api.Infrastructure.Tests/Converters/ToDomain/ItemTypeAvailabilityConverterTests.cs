using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

internal class ItemTypeAvailabilityConverterTests : ToDomainConverterTestBase<ItemTypeAvailableAt, IStoreItemAvailability>
{
    protected override (ItemTypeAvailableAt, IStoreItemAvailability) CreateTestObjects()
    {
        var destination = StoreItemAvailabilityMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    public static ItemTypeAvailableAt GetSource(IStoreItemAvailability destination)
    {
        return new ItemTypeAvailableAt()
        {
            StoreId = destination.StoreId.Value,
            Price = destination.Price,
            DefaultSectionId = destination.DefaultSectionId.Value
        };
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(serviceCollection);
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfNonGenericType(typeof(IStoreItemAvailabilityFactory).Assembly,
            typeof(IStoreItemAvailabilityFactory));
    }
}