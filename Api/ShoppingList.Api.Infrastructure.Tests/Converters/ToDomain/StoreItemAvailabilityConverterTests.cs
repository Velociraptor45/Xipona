using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

public class StoreItemAvailabilityConverterTests : ToDomainConverterTestBase<AvailableAt, IStoreItemAvailability>
{
    protected override (AvailableAt, IStoreItemAvailability) CreateTestObjects()
    {
        var destination = StoreItemAvailabilityMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static AvailableAt GetSource(IStoreItemAvailability destination)
    {
        return new AvailableAt
        {
            StoreId = destination.StoreId.Value,
            Price = destination.Price,
            DefaultSectionId = destination.DefaultSectionId.Value
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(StoreItemAvailabilityConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IStoreItemAvailabilityFactory).Assembly, typeof(IStoreItemAvailabilityFactory));
    }
}