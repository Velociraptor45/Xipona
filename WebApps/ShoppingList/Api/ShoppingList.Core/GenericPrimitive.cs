using System;
using System.Collections.Generic;

namespace ShoppingList.Core
{
    public class GenericPrimitive<T>
        where T : struct
    {
        public T Value { get; }

        public GenericPrimitive(T value)
        {
            this.Value = value;
        }

        public static bool operator ==(GenericPrimitive<T> left, GenericPrimitive<T> right)
        {
            return left.Value.Equals(right?.Value);
        }

        public static bool operator !=(GenericPrimitive<T> left, GenericPrimitive<T> right)
        {
            return !left.Value.Equals(right.Value);
        }

        public override bool Equals(object obj)
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