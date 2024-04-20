using ProjectHermes.Xipona.Api.Core.Attributes;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public enum QuantityTypeInPacket
{
    [QuantityLabel("x")]
    Unit = 0,

    [QuantityLabel("g")]
    Weight = 1,

    [QuantityLabel("ml")]
    Fluid = 2
}