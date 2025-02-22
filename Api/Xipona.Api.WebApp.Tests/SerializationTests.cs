using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.WebApp.Serialization;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.WebApp;

public class SerializationTests
{
    [Fact]
    public void Serialization_ShouldContainAllContracts()
    {
        // Arrange
        var responseTypes = GetAllResponseTypes();

        var parameterTypes = GetContractTypesForEndpoint(typeof(ItemCategoryEndpoints))
            .Concat(GetContractTypesForEndpoint(typeof(ItemEndpoints)))
            .Concat(GetContractTypesForEndpoint(typeof(ManufacturerEndpoints)))
            .Concat(GetContractTypesForEndpoint(typeof(MonitoringEndpoints)))
            .Concat(GetContractTypesForEndpoint(typeof(RecipeEndpoints)))
            .Concat(GetContractTypesForEndpoint(typeof(RecipeTagEndpoints)))
            .Concat(GetContractTypesForEndpoint(typeof(ShoppingListEndpoints)))
            .Concat(GetContractTypesForEndpoint(typeof(StoreEndpoints)))
            .ToList();

        var allContractTypes = responseTypes.Concat(parameterTypes).Distinct().ToList();

        var serializedProperties = typeof(XiponaJsonSerializationContext).GetProperties()
            .Where(p => p.PropertyType.IsGenericType)
            .ToList();

        // Act
        var attributedTypes = allContractTypes
            .Where(t =>
            {
                var prop = serializedProperties
                    .Where(p => p.PropertyType.GenericTypeArguments[0] == t)
                    .ToList();
                return prop.Count == 0;
            })
            .ToList();

        // Assert
        attributedTypes.Should().BeEmpty();
    }

    public static List<Type> GetAllResponseTypes()
    {
        var builder = WebApplication.CreateSlimBuilder();
        using var app = builder.Build();

        app.RegisterItemCategoryEndpoints();
        app.RegisterItemEndpoints();
        app.RegisterManufacturerEndpoints();
        app.RegisterMonitoringEndpoints();
        app.RegisterRecipeEndpoints();
        app.RegisterShoppingListEndpoints();
        app.RegisterRecipeTagEndpoints();
        app.RegisterStoreEndpoints();

        var field = typeof(WebApplication).GetField("_dataSources", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var value = (List<EndpointDataSource>)field.GetValue(app)!;

        var returnTypes = value[0].Endpoints
            .SelectMany(x => x.Metadata.Select(y => y))
            .Where(m => m is ProducesResponseTypeMetadata rtm && (rtm.StatusCode != 200 || rtm.Type != typeof(IResult)))
            .Cast<ProducesResponseTypeMetadata>()
            .Select(t => t.Type)
            .Where(t => t is not null && t != typeof(void))
            .Distinct()
            .Cast<Type>()
            .ToList();

        var nestedTypes = returnTypes.SelectMany(GetNestedTypes).ToList();
        return returnTypes.Concat(nestedTypes).Distinct().ToList();
    }

    private static List<Type> GetContractTypesForEndpoint(Type endpointType)
    {
        var returnType = typeof(Task<IResult>);
        var methods = endpointType
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.ReturnType == returnType)
            .ToList();
        var parameterTypes = methods
            .SelectMany(m => m.GetParameters())
            .Where(p => p.GetCustomAttribute<FromServicesAttribute>() is null)
            .Select(p => p.ParameterType)
            .Distinct()
            .ToList();

        var nestedTypes = parameterTypes.SelectMany(GetNestedTypes).ToList();
        return parameterTypes.Concat(nestedTypes).ToList();
    }

    private static List<Type> GetNestedTypes(Type type)
    {
        if (!type.Namespace!.StartsWith("ProjectHermes.Xipona") && !type.IsGenericType)
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
