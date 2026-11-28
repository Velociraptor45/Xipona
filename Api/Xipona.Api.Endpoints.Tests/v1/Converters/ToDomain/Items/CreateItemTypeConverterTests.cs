using AutoMapper;
using FluentAssertions.Equivalency;
using Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.Shared;
using Xipona.Api.Core.TestKit.Services;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Models.Factories;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class CreateItemTypeConverterTests
    : ToDomainConverterTestBase<CreateItemTypeContract, IItemType, CreateItemTypeConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);
    private readonly DateTimeOffset _createdAt = DateTimeOffset.UtcNow;

    public override CreateItemTypeConverter CreateSut()
    {
        return new(new ItemTypeFactory(_dateTimeServiceMock.Object), new ItemAvailabilityConverter());
    }

    protected override void AddMapping(IMappingExpression<CreateItemTypeContract, IItemType> mapping)
    {
        mapping.As<ItemType>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<CreateItemTypeContract, ItemType>()
            .ForCtorParam(nameof(ItemType.Id).LowerFirstChar(), opt => opt.MapFrom(_ => ItemTypeId.New))
            .ForCtorParam(nameof(ItemType.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(_ => false))
            .ForCtorParam(nameof(ItemType.CreatedAt).LowerFirstChar(), opt => opt.MapFrom(_ => _createdAt))
            .ForCtorParam(nameof(ItemType.PredecessorId).LowerFirstChar(), opt => opt.MapFrom(_ => (ItemTypeId?)null))
            .ForCtorParam(nameof(ItemType.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
            .ForCtorParam(nameof(ItemType.Availabilities).LowerFirstChar(), opt => opt.MapFrom((src, ctx) =>
                src.Availabilities.Select(ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));

        new ItemAvailabilityConverterTests().AddMapping(cfg);
    }

    protected override void Setup(CreateItemTypeContract source)
    {
        _dateTimeServiceMock.SetupUtcNow(_createdAt);
    }

    protected override void CustomizeAssertionOptions(EquivalencyOptions<IItemType> opt)
    {
        opt.Excluding(info => info.Path == "Id");
    }
}