﻿namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "<Pending>")]
public class GenericPrimitive<T> : IEquatable<GenericPrimitive<T>>
        where T : struct
{
    public T Value { get; }

    public GenericPrimitive(T value)
    {
        Value = value;
    }

    public static bool operator ==(GenericPrimitive<T> left, GenericPrimitive<T> right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Value.Equals(right?.Value);
    }

    public static bool operator !=(GenericPrimitive<T> left, GenericPrimitive<T> right)
    {
        if (left is null)
        {
            return !(right is null);
        }

        return !left.Value.Equals(right?.Value);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is GenericPrimitive<T> other && Equals(other);
    }

    public bool Equals(GenericPrimitive<T>? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}