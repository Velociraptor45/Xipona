using AutoFixture.Kernel;

namespace Xipona.Api.Domain.TestKit.Common.AutoFixture.Selectors;

public class BiggestConstructorQuery : IMethodQuery
{
    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        var ctors = type.GetConstructors();
        var ctor = ctors.MaxBy(x => x.GetParameters().Length);

        return [new ConstructorMethod(ctor)];
    }
}
