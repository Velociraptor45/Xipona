using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Converters.Items.ToContract;

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