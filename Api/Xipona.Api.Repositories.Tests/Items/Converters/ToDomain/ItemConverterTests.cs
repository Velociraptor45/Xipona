using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using System;

using DomainModels = ProjectHermes.Xipona.Api.Domain.Items.Models;

using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemConverterTests
{
    public class WithItemTypes : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
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
                        new ItemTypes(src.ItemTypes.Select(ctx.Mapper.Map<ItemType, IItemType>), new ItemTypeFactory(ItemTypeDateTimeServiceMock.Object)),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value),
                        src.CreatedAt));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemTypeConverterTests().AddMapping(cfg);
        }

        protected override void Setup(Item source)
        {
            ItemDateTimeServiceMock.SetupUtcNow(source.CreatedAt);

            var sequence = ItemDateTimeServiceMock.SetupSequence(x => x.UtcNow);
            foreach (var type in source.ItemTypes)
            {
                sequence.Returns(type.CreatedAt);
            }
        }
    }

    public class WithoutItemCategoryId : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().WithoutItemCategoryId().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
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
                        null,
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value),
                        src.CreatedAt));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }

        protected override void Setup(Item source)
        {
            ItemDateTimeServiceMock.SetupUtcNow(source.CreatedAt);
        }
    }

    public class WithoutManufacturerId : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().WithoutManufacturerId().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
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
                        null,
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value),
                        src.CreatedAt));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }

        protected override void Setup(Item source)
        {
            ItemDateTimeServiceMock.SetupUtcNow(source.CreatedAt);
        }
    }

    public class WithoutItemTypes : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
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
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value),
                        src.CreatedAt));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }

        protected override void Setup(Item source)
        {
            ItemDateTimeServiceMock.SetupUtcNow(source.CreatedAt);
        }
    }

    public class WithoutTemporaryItemId : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().WithoutCreatedFrom().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
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
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        null,
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value),
                        src.CreatedAt));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }

        protected override void Setup(Item source)
        {
            ItemDateTimeServiceMock.SetupUtcNow(source.CreatedAt);
        }
    }

    public class WithoutQuantityInPacket : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder()
                .WithEmptyItemTypes()
                .WithoutQuantityInPacket()
                .WithoutQuantityTypeInPacket()
                .WithQuantityType(QuantityType.Weight.ToInt())
                .Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(), null),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value),
                        src.CreatedAt));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }

        protected override void Setup(Item source)
        {
            ItemDateTimeServiceMock.SetupUtcNow(source.CreatedAt);
        }
    }

    public class ItemTypesWithoutItemCategoryId : ItemFailedConverterTestsBase<InvalidOperationException>
    {
        protected override string ExpectedMessage => "ItemCategoryId mustn't be null for an item with types";

        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().WithoutItemCategoryId().Create();
        }
    }

    public class QuantityInPacketButNoQuantityInPacketType : ItemFailedConverterTestsBase<InvalidOperationException>
    {
        protected override string ExpectedMessage => "Invalid data state for item *: QuantityInPacket isn't null but QuantityTypeInPacket is";

        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().WithoutQuantityTypeInPacket().Create();
        }
    }

    public class QuantityInPacketTypeButNoQuantityInPacket : ItemFailedConverterTestsBase<InvalidOperationException>
    {
        protected override string ExpectedMessage => "Invalid data state for item *: QuantityInPacket is null but QuantityTypeInPacket isn't";

        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().WithoutQuantityInPacket().Create();
        }
    }

    public abstract class ItemConverterTestsBase : ToDomainConverterTestBase<Item, IItem, ItemConverter>
    {
        protected readonly DateTimeServiceMock ItemDateTimeServiceMock = new(MockBehavior.Strict);
        protected readonly DateTimeServiceMock ItemTypeDateTimeServiceMock = new(MockBehavior.Strict);

        public override ItemConverter CreateSut()
        {
            return new ItemConverter(
                new ItemFactory(new ItemTypeFactory(ItemTypeDateTimeServiceMock.Object), ItemDateTimeServiceMock.Object),
                new ItemTypeConverterTests().CreateSut(),
                new ItemAvailabilityConverterTests().CreateSut());
        }
    }

    public abstract class ItemFailedConverterTestsBase<TException> : ToDomainFailedConverterTestBase<Item, IItem, ItemConverter, TException>
        where TException : Exception
    {
        protected readonly DateTimeServiceMock ItemDateTimeServiceMock = new(MockBehavior.Strict);
        protected readonly DateTimeServiceMock ItemTypeDateTimeServiceMock = new(MockBehavior.Strict);

        public override ItemConverter CreateSut()
        {
            return new ItemConverter(
                new ItemFactory(new ItemTypeFactory(ItemTypeDateTimeServiceMock.Object), ItemDateTimeServiceMock.Object),
                new ItemTypeConverterTests().CreateSut(),
                new ItemAvailabilityConverterTests().CreateSut());
        }
    }
}