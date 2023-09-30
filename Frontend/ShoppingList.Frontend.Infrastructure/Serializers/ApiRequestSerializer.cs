using Blazored.SessionStorage.Serialization;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Serializers;

public class ApiRequestSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public ApiRequestSerializer()
    {
        _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public string Serialize<T>(T obj)
    {
        if (obj is not List<IApiRequest> requests)
        {
            throw new NotSupportedException($"Type {typeof(T)} is not supported by {nameof(ApiRequestSerializer)}");
        }

        var list = new List<RequestWrapper>();

        foreach (var request in requests)
        {
            var type = request.GetType();
            var requestObj = Convert.ChangeType(request, type);

            var requestSerialize = JsonSerializer.Serialize(requestObj, _options);

            var wrapper = new RequestWrapper()
            {
                TypeName = type.AssemblyQualifiedName!,
                Request = requestSerialize
            };

            list.Add(wrapper);
        }

        return JsonSerializer.Serialize(list, _options);
    }

    public T Deserialize<T>(string text)
    {
        var list = JsonSerializer.Deserialize<List<RequestWrapper>>(text, _options);

        var output = new List<IApiRequest>();
        foreach (var wrapper in list)
        {
            var type = Type.GetType(wrapper.TypeName)!;

            var typedRequest = JsonSerializer.Deserialize(wrapper.Request, type, _options);
            output.Add((IApiRequest)typedRequest);
        }

        return (T)(object)output;
    }

    private sealed record RequestWrapper
    {
        public string TypeName { get; init; } = string.Empty;
        public string Request { get; init; } = string.Empty;
    }
}