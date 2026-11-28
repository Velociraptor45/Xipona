using AutoFixture.Kernel;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.TestKit.Common.AutoFixture.Selectors;

public class ItemConstructorQuery : IMethodQuery
{
    private readonly Type _availabilitiesType;

    public ItemConstructorQuery()
    {
        _availabilitiesType = typeof(IEnumerable<ItemAvailability>);
    }

    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        var ctors = type.GetConstructors();
        var ctor = ctors.Single(ctor => ctor.GetParameters().Any(p => p.ParameterType == _availabilitiesType));

        return [new ConstructorMethod(ctor)];
    }
}