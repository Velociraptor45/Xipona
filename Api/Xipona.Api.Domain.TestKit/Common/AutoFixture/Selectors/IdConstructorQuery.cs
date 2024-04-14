using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Core.Extensions;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Common.AutoFixture.Selectors;

public class IdConstructorQuery : IMethodQuery
{
    private readonly List<Type> _types;

    public IdConstructorQuery()
    {
        _types = new List<Type>
        {
            typeof(int),
            typeof(Guid),
        };
    }

    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        var ctors = type.GetConstructors();
        var ctor = ctors.Single(ctor =>
            ctor.GetParameters().Length == 1 &&
            ctor.GetParameters().Any(p => _types.Contains(p.ParameterType)));

        return new ConstructorMethod(ctor).ToMonoList();
    }
}