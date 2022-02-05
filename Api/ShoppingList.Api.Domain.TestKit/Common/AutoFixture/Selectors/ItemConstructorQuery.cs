using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;

public class ItemConstructorQuery : IMethodQuery
{
    private readonly Type _availabilitiesType;

    public ItemConstructorQuery()
    {
        _availabilitiesType = typeof(IEnumerable<IStoreItemAvailability>);
    }

    public IEnumerable<IMethod> SelectMethods(Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        var ctors = type.GetConstructors();
        var ctor = ctors.Single(ctor => ctor.GetParameters().Any(p => p.ParameterType == _availabilitiesType));

        return new ConstructorMethod(ctor).ToMonoList();
    }
}