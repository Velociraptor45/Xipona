using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Recipes.ToDomain;

public class AddToShoppingListAvailabilityConverterTests
    : ToDomainConverterBase<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability, AddToShoppingListAvailabilityConverter>
{
    protected override AddToShoppingListAvailabilityConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability> mapping)
    {
        mapping
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.StoreName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
    }
}