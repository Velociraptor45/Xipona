using AutoMapper;
using Xipona.Api.Contracts.Items.Commands.Shared;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class ItemAvailabilityConverterTests :
    ToDomainConverterTestBase<ItemAvailabilityContract, ItemAvailability, ItemAvailabilityConverter>
{
    public override ItemAvailabilityConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<ItemAvailabilityContract, ItemAvailability> mapping)
    {
        mapping
            .ForCtorParam(nameof(ItemAvailability.StoreId), opt => opt.MapFrom(src => new StoreId(src.StoreId)))
            .ForCtorParam(nameof(ItemAvailability.Price), opt => opt.MapFrom(src => new Price(src.Price)))
            .ForCtorParam(nameof(ItemAvailability.DefaultSectionId), opt => opt.MapFrom(src => new SectionId(src.DefaultSectionId)));
    }
}