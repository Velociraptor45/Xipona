using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ItemType = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.ItemType;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

public class ItemTypeConverterTests : ToDomainConverterTestBase<ItemType, IItemType>
{
    protected override (ItemType, IItemType) CreateTestObjects()
    {
        var destination = new ItemTypeBuilder().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    public static ItemType GetSource(IItemType destination)
    {
        var availabilities = destination.Availabilities
            .Select(ItemTypeAvailabilityConverterTests.GetSource)
            .ToList();

        return new ItemType
        {
            Id = destination.Id.Value,
            Name = destination.Name.Value,
            AvailableAt = availabilities
        };
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemTypeConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemTypeFactory).Assembly, typeof(IItemTypeFactory));

        ItemTypeAvailabilityConverterTests.AddDependencies(serviceCollection);
    }
}