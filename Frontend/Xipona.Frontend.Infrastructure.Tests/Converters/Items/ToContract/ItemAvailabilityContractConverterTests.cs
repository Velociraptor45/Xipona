using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Items.ToContract;

public class ItemAvailabilityContractConverterTests :
    ToContractConverterBase<EditedItemAvailability, ItemAvailabilityContract, ItemAvailabilityContractConverter>
{
    protected override ItemAvailabilityContractConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<EditedItemAvailability, ItemAvailabilityContract> mapping)
    {
        mapping
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerQuantity))
            .ForMember(dest => dest.DefaultSectionId, opt => opt.MapFrom(src => src.DefaultSectionId));
    }
}