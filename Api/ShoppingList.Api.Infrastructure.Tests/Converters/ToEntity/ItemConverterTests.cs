using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using System.Linq;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class ItemConverterTests : ToEntityConverterTestBase<IStoreItem, Item>
    {
        protected override (IStoreItem, Item) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var availabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            var storeItemFixture = new StoreItemFixture(availabilityFixture, commonFixture);

            var source = storeItemFixture.CreateValid();
            var destination = GetDestination(source);

            return (source, destination);
        }

        public static Item GetDestination(IStoreItem source)
        {
            return new Item
            {
                Id = source.Id?.Value ?? 0,
                Name = source.Name,
                Deleted = source.IsDeleted,
                Comment = source.Comment,
                IsTemporary = source.IsTemporary,
                QuantityType = source.QuantityType.ToInt(),
                QuantityInPacket = source.QuantityInPacket,
                QuantityTypeInPacket = source.QuantityTypeInPacket.ToInt(),
                ItemCategoryId = source.ItemCategoryId?.Value,
                ManufacturerId = source.ManufacturerId?.Value,
                CreatedFrom = source.TemporaryId?.Value,
                AvailableAt = source.Availabilities
                    .Select(av =>
                        new AvailableAt()
                        {
                            StoreId = av.StoreId.Value,
                            Price = av.Price,
                            ItemId = source.Id?.Value ?? 0,
                            DefaultSectionId = av.DefaultSectionId.Value
                        }).ToList(),
                PredecessorId = source.Predecessor?.Id.Value
            };
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ItemConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}