using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.WebApp.Serialization;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.WebApp;

public class SerializationTests
{
    // todo: convert this to endpoints
    [Fact]
    public void Serialization_ShouldContainAllContracts()
    {
        // Arrange
        var contractTypes = GetTypesForController(typeof(StoreController))
            .Concat(GetTypesForController(typeof(ShoppingListController)))
            .Concat(GetTypesForController(typeof(RecipeController)))
            .Concat(GetTypesForController(typeof(RecipeTagController)))
            .Concat(GetTypesForController(typeof(MonitoringController)))
            .Distinct()
            .ToList();

        // Act
        var attributedTypes = contractTypes
            .Where(t =>
            {
                var prop = typeof(XiponaJsonSerializationContext).GetProperties()
                    .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GenericTypeArguments[0] == t)
                    .ToList();
                return prop.Count == 0;
            })
            .ToList();

        // Assert
        attributedTypes.Should().BeEmpty();
    }

    private List<Type> GetTypesForController(Type controllerType)
    {
        var returnType = typeof(Task<IActionResult>);
        var methods = controllerType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(m => m.ReturnType == returnType)
            .ToList();
        var methodReturnTypes = methods
            .SelectMany(m => m.GetCustomAttributes(typeof(ProducesResponseTypeAttribute)))
            .Cast<ProducesResponseTypeAttribute>()
            .Select(a => a.Type)
            .Where(t => t != typeof(void))
            .ToList();
        var parameterTypes = methods
            .SelectMany(m => m.GetParameters())
            .Select(p => p.ParameterType)
            .ToList();

        var allTypes = methodReturnTypes.Concat(parameterTypes).ToList();
        var nestedTypes = allTypes.SelectMany(GetNestedTypes).ToList();
        return allTypes.Concat(nestedTypes).ToList();
    }

    private List<Type> GetNestedTypes(Type type)
    {
        if (!type.Namespace!.StartsWith("ProjectHermes.Xipona"))
            return [];

        var properties = type.GetProperties()
            .Select(p => p.PropertyType)
            .ToList();

        var nestedTypes = properties
            .Select(t =>
            {
                if (!t.IsGenericType)
                    return t;

                if (t.GenericTypeArguments.Length == 1)
                {
                    return t.GenericTypeArguments[0];
                }

                throw new NotImplementedException($"Handling of {t} not implemented");

            })
            .SelectMany(GetNestedTypes).ToList();

        return properties.Concat(nestedTypes).ToList();
    }
}
