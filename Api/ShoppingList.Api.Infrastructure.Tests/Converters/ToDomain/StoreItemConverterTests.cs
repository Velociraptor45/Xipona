using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using Item = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.Item;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

public class StoreItemConverterTests : ToDomainConverterTestBase<Item, IItem>
{
    protected override (Item, IItem) CreateTestObjects()
    {
        var destination = StoreItemMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static Item GetSource(IItem destination)
    {
        Item predecessor = null;
        if (destination.Predecessor != null)
            predecessor = GetSource(destination.Predecessor);

        var availabilities = destination.Availabilities
            .Select(StoreItemAvailabilityConverterTests.GetSource)
            .ToList();

        var itemTypes = destination.ItemTypes
            .Select(ItemTypeConverterTests.GetSource)
            .ToList();

        return new Item
        {
            Id = destination.Id.Value,
            Name = destination.Name.Value,
            Deleted = destination.IsDeleted,
            Comment = destination.Comment.Value,
            IsTemporary = destination.IsTemporary,
            QuantityType = destination.ItemQuantity.Type.ToInt(),
            QuantityInPacket = destination.ItemQuantity.InPacket?.Quantity.Value,
            QuantityTypeInPacket = destination.ItemQuantity.InPacket?.Type.ToInt(),
            ItemCategoryId = destination.ItemCategoryId?.Value,
            ManufacturerId = destination.ManufacturerId?.Value,
            PredecessorId = predecessor?.Id,
            Predecessor = predecessor,
            AvailableAt = availabilities,
            CreatedFrom = destination.TemporaryId?.Value,
            ItemTypes = itemTypes
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(StoreItemConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemFactory).Assembly, typeof(IItemFactory));

        StoreItemAvailabilityConverterTests.AddDependencies(serviceCollection);
        ManufacturerConverterTests.AddDependencies(serviceCollection);
        ItemCategoryConverterTests.AddDependencies(serviceCollection);
        ItemTypeConverterTests.AddDependencies(serviceCollection);
    }
}