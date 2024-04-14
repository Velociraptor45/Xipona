using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToEntity;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToEntity;

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
                        StoreId = av.StoreId,
                        Price = av.Price,
                        ItemId = source.Id,
                        DefaultSectionId = av.DefaultSectionId
                    }).ToList(),
            PredecessorId = source.PredecessorId,
            ItemTypes = new List<ItemType>(),
            UpdatedOn = source.UpdatedOn,
            CreatedAt = source.CreatedAt
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ItemConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}