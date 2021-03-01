using System;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Core.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttribute<TAttribute>();
        }

        public static int ToInt<T>(this T value)
            where T : Enum
        {
            int intValue = (int)(object)value;

            if (!Enum.IsDefined(typeof(T), intValue))
                throw new InvalidOperationException($"{intValue} is an invalid {typeof(T).FullName} value.");

            return intValue;
        }
    }
}