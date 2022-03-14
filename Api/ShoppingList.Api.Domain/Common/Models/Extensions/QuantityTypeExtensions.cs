using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;

public static class QuantityTypeExtensions
{
    public static QuantityTypeReadModel ToReadModel(this QuantityType quantityType)
    {
        return new QuantityTypeReadModel(
            (int)quantityType,
            quantityType.ToString(),
            quantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
            quantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
            quantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel,
            quantityType.GetAttribute<QuantityNormalizerAttribute>().Value);
    }
}