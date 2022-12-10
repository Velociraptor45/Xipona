using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

public class ItemAvailabilityConverterTests : ToDomainConverterTestBase<AvailableAt, IItemAvailability>
{
    protected override (AvailableAt, IItemAvailability) CreateTestObjects()
    {
        var destination = ItemAvailabilityMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static AvailableAt GetSource(IItemAvailability destination)
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
        serviceCollection.AddImplementationOfGenericType(typeof(ItemAvailabilityConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemAvailabilityFactory).Assembly, typeof(IItemAvailabilityFactory));
    }
}