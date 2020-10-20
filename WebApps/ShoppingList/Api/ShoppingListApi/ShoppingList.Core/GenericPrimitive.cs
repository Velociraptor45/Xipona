namespace ShoppingList.Core
{
    public class GenericPrimitive<T>
        where T : struct
    {
        protected readonly T value;

        public GenericPrimitive(T value)
        {
            this.value = value;
        }

        public static bool operator ==(GenericPrimitive<T> left, GenericPrimitive<T> right)
        {
            return left.value.Equals(right.value);
        }

        public static bool operator !=(GenericPrimitive<T> left, GenericPrimitive<T> right)
        {
            return !left.value.Equals(right.value);
        }
    }
}