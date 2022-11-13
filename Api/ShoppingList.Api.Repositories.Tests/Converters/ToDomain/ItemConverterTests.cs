using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

public class ItemConverterTests : ToDomainConverterTestBase<Item, IItem>
{
    protected override (Item, IItem) CreateTestObjects()
    {
        var destination = ItemMother.Initial().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static Item GetSource(IItem destination)
    {
        var availabilities = destination.Availabilities
            .Select(ItemAvailabilityConverterTests.GetSource)
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
            PredecessorId = destination.PredecessorId?.Value,
            AvailableAt = availabilities,
            CreatedFrom = destination.TemporaryId?.Value,
            ItemTypes = itemTypes,
            UpdatedOn = destination.UpdatedOn
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemFactory).Assembly, typeof(IItemFactory));

        ItemAvailabilityConverterTests.AddDependencies(serviceCollection);
        ManufacturerConverterTests.AddDependencies(serviceCollection);
        ItemCategoryConverterTests.AddDependencies(serviceCollection);
        ItemTypeConverterTests.AddDependencies(serviceCollection);
    }
}