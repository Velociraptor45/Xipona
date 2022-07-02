using AutoFixture.Kernel;
using System.Reflection;

namespace ShoppingList.Api.TestTools.AutoFixture;

public class PropertyRelay<TTarget, TValueType> : ISpecimenBuilder
{
    private readonly TValueType _value;

    public PropertyRelay(string propertyName, TValueType value)
    {
        PropertyName = propertyName;
        _value = value;
    }

    public string PropertyName { get; }

    public object Create(object request, ISpecimenContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        PropertyInfo? parameter = request as PropertyInfo;
        if (parameter == null)
            return new NoSpecimen();
        if (parameter.DeclaringType != typeof(TTarget) ||
            parameter.MemberType != MemberTypes.Property ||
            parameter.PropertyType != typeof(TValueType) ||
            parameter.Name != PropertyName)
            return new NoSpecimen();
        return _value!;
    }
}