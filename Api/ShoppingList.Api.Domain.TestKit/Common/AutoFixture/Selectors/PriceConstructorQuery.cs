using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;

public class PriceConstructorQuery : IMethodQuery
{
    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        var ctors = type.GetConstructors();
        var ctor = ctors.Single(ctor =>
            ctor.GetParameters().Length == 1 &&
            ctor.GetParameters().Any(p => p.ParameterType == typeof(float)));

        return new ConstructorMethod(ctor).ToMonoList();
    }
}