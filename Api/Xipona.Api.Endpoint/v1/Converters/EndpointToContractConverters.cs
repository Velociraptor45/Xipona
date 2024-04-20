﻿using ProjectHermes.Xipona.Api.Core.Converter;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters;

public class EndpointToContractConverters : Dictionary<(Type, Type), IToContractConverter>
{
    public IEnumerable<TContract> ToContract<TDomain, TContract>(IEnumerable<TDomain> domain)
    {
        var typedConverter = GetConverter<TDomain, TContract>();

        return typedConverter.ToContract(domain);
    }

    public TContract ToContract<TDomain, TContract>(TDomain domain)
    {
        var typedConverter = GetConverter<TDomain, TContract>();

        return typedConverter.ToContract(domain);
    }

    private IToContractConverter<TDomain, TContract> GetConverter<TDomain, TContract>()
    {
        var domainType = typeof(TDomain);
        var contractType = typeof(TContract);

        if (!TryGetValue((domainType, contractType), out var converter))
        {
            throw new InvalidOperationException($"No converter for {domainType} to {contractType} found.");
        }

        var typedConverter = converter as IToContractConverter<TDomain, TContract>;
        if (typedConverter == null)
            throw new InvalidOperationException($"Registered converter for {domainType} to {contractType} does not convert said types.");

        return typedConverter;
    }
}