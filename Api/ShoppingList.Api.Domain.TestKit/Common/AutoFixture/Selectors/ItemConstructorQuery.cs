using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;

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

        return new ConstructorMethod(ctor).ToMonoList();
    }
}