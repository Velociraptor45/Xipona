using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

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
            AvailableAt = availabilities,
            PredecessorId = destination.PredecessorId?.Value
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