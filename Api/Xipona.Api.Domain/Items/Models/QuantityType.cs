using ProjectHermes.Xipona.Api.Core.Attributes;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public enum QuantityType
{
    [PriceLabel("€")]
    [DefaultQuantity(1)]
    [QuantityLabel("x")]
    [QuantityNormalizer(1)]
    Unit = 0,

    [PriceLabel("€/kg")]
    [DefaultQuantity(100)]
    [QuantityLabel("g")]
    [QuantityNormalizer(1000)]
    Weight = 1
}