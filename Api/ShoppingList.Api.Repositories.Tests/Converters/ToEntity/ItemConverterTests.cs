using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class ItemConverterTests : ToEntityConverterTestBase<IItem, Item>
{
    protected override (IItem, Item) CreateTestObjects()
    {
        var source = ItemMother.Initial().Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static Item GetDestination(IItem source)
    {
        return new Item
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Comment = source.Comment.Value,
            IsTemporary = source.IsTemporary,
            QuantityType = source.ItemQuantity.Type.ToInt(),
            QuantityInPacket = source.ItemQuantity.InPacket?.Quantity,
            QuantityTypeInPacket = source.ItemQuantity.InPacket?.Type.ToInt(),
            ItemCategoryId = source.ItemCategoryId,
            ManufacturerId = source.ManufacturerId,
            CreatedFrom = source.TemporaryId,
            AvailableAt = source.Availabilities
                .Select(av =>
                    new AvailableAt
                    {
                        StoreId = av.StoreId.Value,
                        Price = av.Price,
                        ItemId = source.Id,
                        DefaultSectionId = av.DefaultSectionId
                    }).ToList(),
            PredecessorId = source.PredecessorId,
            ItemTypes = new List<ItemType>(),
            UpdatedOn = source.UpdatedOn
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ItemConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}