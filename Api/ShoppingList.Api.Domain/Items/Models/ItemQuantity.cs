using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public sealed class ItemQuantity : IEquatable<ItemQuantity>
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

    public bool Equals(ItemQuantity? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Type == other.Type && Equals(InPacket, other.InPacket);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ItemQuantity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, InPacket);
    }

    public static bool operator ==(ItemQuantity? left, ItemQuantity? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ItemQuantity? left, ItemQuantity? right)
    {
        return !Equals(left, right);
    }
}