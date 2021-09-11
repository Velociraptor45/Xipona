using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Core
{
    public class GenericPrimitive<T>
        where T : struct
    {
        public T Value { get; }

        public GenericPrimitive(T value)
        {
            Value = value;
        }

#pragma warning disable S3875 // "operator==" should not be overloaded on reference types

        public static bool operator ==(GenericPrimitive<T>? left, GenericPrimitive<T>? right)
#pragma warning restore S3875 // "operator==" should not be overloaded on reference types
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Value.Equals(right?.Value);
        }

        public static bool operator !=(GenericPrimitive<T>? left, GenericPrimitive<T>? right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            return !left.Value.Equals(right?.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is GenericPrimitive<T> primitive &&
                   EqualityComparer<T>.Default.Equals(Value, primitive.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}