using AutoFixture.Kernel;
using System.Reflection;

namespace ProjectHermes.Xipona.Frontend.TestTools.AutoFixture;

public class ConstructorArgumentRelay<TTarget, TValueType> : ISpecimenBuilder
{
    private readonly TValueType _value;

    public ConstructorArgumentRelay(string paramName, TValueType value)
    {
        ParamName = paramName;
        _value = value;
    }

    public string ParamName { get; }

    public object Create(object request, ISpecimenContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (request is not ParameterInfo parameter)
            return new NoSpecimen();
        if (parameter.Member.DeclaringType != typeof(TTarget) ||
            parameter.Member.MemberType != MemberTypes.Constructor ||
            parameter.ParameterType != typeof(TValueType) ||
            parameter.Name != ParamName)
            return new NoSpecimen();
        return _value!;
    }
}