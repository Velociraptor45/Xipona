using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToDomain;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

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
            Id = destination.Id,
            Name = destination.Name,
            Deleted = destination.IsDeleted,
            Comment = destination.Comment.Value,
            IsTemporary = destination.IsTemporary,
            QuantityType = destination.ItemQuantity.Type.ToInt(),
            QuantityInPacket = destination.ItemQuantity.InPacket?.Quantity,
            QuantityTypeInPacket = destination.ItemQuantity.InPacket?.Type.ToInt(),
            ItemCategoryId = destination.ItemCategoryId,
            ManufacturerId = destination.ManufacturerId,
            PredecessorId = destination.PredecessorId,
            AvailableAt = availabilities,
            CreatedFrom = destination.TemporaryId,
            ItemTypes = itemTypes,
            UpdatedOn = destination.UpdatedOn,
            CreatedAt = destination.CreatedAt
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