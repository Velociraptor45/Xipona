using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.Items.Models;

using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemConverterTests
{
    public class WithItemTypes : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().Create();
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Item, DomainModels.Item>()
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        new ItemTypes(src.ItemTypes.Select(ctx.Mapper.Map<ItemType, IItemType>), new ItemTypeFactory()),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));

            new ItemTypeConverterTests().AddMapping(cfg);
        }
    }

    public class WithoutItemTypes : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().Create();
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Item, DomainModels.Item>()
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, IItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));

            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public abstract class ItemConverterTestsBase : ToDomainConverterBase<Item, IItem, ItemConverter>
    {
        public override ItemConverter CreateSut()
        {
            return new ItemConverter(
                new ItemFactory(new ItemTypeFactory()),
                new ItemTypeConverterTests().CreateSut(),
                new ItemAvailabilityConverterTests().CreateSut());
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping.As<DomainModels.Item>();
        }
    }
}