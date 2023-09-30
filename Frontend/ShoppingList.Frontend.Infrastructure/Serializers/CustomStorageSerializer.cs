using Blazored.SessionStorage.Serialization;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Serializers;

public class CustomStorageSerializer : IJsonSerializer
{
    private readonly ApiRequestSerializer _apiRequestSerializer = new();
    private static readonly Type _apiRequestsType = typeof(List<IApiRequest>);

    public string Serialize<T>(T obj)
    {
        if (obj is List<IApiRequest>)
        {
            return _apiRequestSerializer.Serialize(obj);
        }

        return JsonSerializer.Serialize(obj);
    }

    public T Deserialize<T>(string text)
    {
        if (typeof(T) == _apiRequestsType)
        {
            return _apiRequestSerializer.Deserialize<T>(text);
        }

        return JsonSerializer.Deserialize<T>(text);
    }
}