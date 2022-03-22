namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public sealed class ItemQuantityInPacket : IEquatable<ItemQuantityInPacket>
{
    public ItemQuantityInPacket(Quantity quantity, QuantityTypeInPacket type)
    {
        Quantity = quantity;
        Type = type;
    }

    public Quantity Quantity { get; }
    public QuantityTypeInPacket Type { get; }

    public bool Equals(ItemQuantityInPacket? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Quantity.Equals(other.Quantity) && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ItemQuantityInPacket other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Quantity, (int)Type);
    }

    public static bool operator ==(ItemQuantityInPacket? left, ItemQuantityInPacket? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ItemQuantityInPacket? left, ItemQuantityInPacket? right)
    {
        return !Equals(left, right);
    }
}