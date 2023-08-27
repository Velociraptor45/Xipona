using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public record ItemQuantity
{
    public ItemQuantity(QuantityType type, ItemQuantityInPacket? inPacket)
    {
        Type = type;
        InPacket = inPacket;
        EnsureIsValid();
    }

    public QuantityType Type { get; }
    public ItemQuantityInPacket? InPacket { get; }

    private void EnsureIsValid()
    {
        switch (Type)
        {
            case QuantityType.Unit when InPacket is null:
                throw new DomainException(new QuantityTypeHasNoInPacketValuesReason(Type));
            case QuantityType.Weight when InPacket is not null:
                throw new DomainException(new QuantityTypeHasInPacketValuesReason(Type));
        }
    }
}