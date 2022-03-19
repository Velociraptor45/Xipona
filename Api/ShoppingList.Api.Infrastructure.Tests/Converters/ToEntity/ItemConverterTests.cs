using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity;

public class ItemConverterTests : ToEntityConverterTestBase<IStoreItem, Item>
{
    protected override (IStoreItem, Item) CreateTestObjects()
    {
        var source = StoreItemMother.Initial().Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static Item GetDestination(IStoreItem source)
    {
        return new Item
        {
            Id = source.Id.Value,
            Name = source.Name.Value,
            Deleted = source.IsDeleted,
            Comment = source.Comment.Value,
            IsTemporary = source.IsTemporary,
            QuantityType = source.ItemQuantity.Type.ToInt(),
            QuantityInPacket = source.ItemQuantity.InPacket.Quantity.Value,
            QuantityTypeInPacket = source.ItemQuantity.InPacket.Type.ToInt(),
            ItemCategoryId = source.ItemCategoryId?.Value,
            ManufacturerId = source.ManufacturerId?.Value,
            CreatedFrom = source.TemporaryId?.Value,
            AvailableAt = source.Availabilities
                .Select(av =>
                    new AvailableAt
                    {
                        StoreId = av.StoreId.Value,
                        Price = av.Price.Value,
                        ItemId = source.Id.Value,
                        DefaultSectionId = av.DefaultSectionId.Value
                    }).ToList(),
            PredecessorId = source.Predecessor?.Id.Value,
            ItemTypes = new List<ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemType>()
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ItemConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}