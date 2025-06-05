using AutoMapper;
using Xipona.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using Xipona.Api.Contracts.Items.Commands.Shared;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Core.TestKit;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.TemporaryItems;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class MakeTemporaryItemPermanentCommandConverterTests
{
    public class WithQuantityWeight : MakeTemporaryItemPermanentCommandConverterTestsBase
    {
        protected override (Guid id, MakeTemporaryItemPermanentContract contract) CreateSource()
        {
            var contract = new TestBuilder<MakeTemporaryItemPermanentContract>()
                .FillPropertyWith(x => x.QuantityInPacket, null)
                .FillPropertyWith(x => x.QuantityTypeInPacket, null)
                .FillPropertyWith(x => x.QuantityType, QuantityType.Weight.ToInt())
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(MakeTemporaryItemPermanentCommand.PermanentItem).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new PermanentItem(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Weight, null),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.Availabilities.Select(
                            ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithQuantityUnit : MakeTemporaryItemPermanentCommandConverterTestsBase
    {
        protected override (Guid id, MakeTemporaryItemPermanentContract contract) CreateSource()
        {
            var contract = new TestBuilder<MakeTemporaryItemPermanentContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(MakeTemporaryItemPermanentCommand.PermanentItem).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new PermanentItem(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.Availabilities.Select(
                            ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithManufacturerNull : MakeTemporaryItemPermanentCommandConverterTestsBase
    {
        protected override (Guid id, MakeTemporaryItemPermanentContract contract) CreateSource()
        {
            var contract = new TestBuilder<MakeTemporaryItemPermanentContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .FillPropertyWith(x => x.ManufacturerId, null)
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(MakeTemporaryItemPermanentCommand.PermanentItem).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new PermanentItem(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        null,
                        src.contract.Availabilities.Select(
                            ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public abstract class MakeTemporaryItemPermanentCommandConverterTestsBase :
        ToDomainConverterTestBase<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand, MakeTemporaryItemPermanentCommandConverter>
    {
        public override MakeTemporaryItemPermanentCommandConverter CreateSut()
        {
            return new(new ItemAvailabilityConverter());
        }
    }
}