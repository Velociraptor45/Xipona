﻿namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters;

public class EndpointConverters : IEndpointConverters
{
    private readonly EndpointToDomainConverters _toDomainConverters;
    private readonly EndpointToContractConverters _toContractConverters;

    public EndpointConverters(EndpointToDomainConverters toDomainConverters,
        EndpointToContractConverters toContractConverters)
    {
        _toDomainConverters = toDomainConverters;
        _toContractConverters = toContractConverters;
    }

    public TDomain ToDomain<TContract, TDomain>(TContract contract)
    {
        return _toDomainConverters.ToDomain<TContract, TDomain>(contract);
    }

    public IEnumerable<TDomain> ToDomain<TContract, TDomain>(IEnumerable<TContract> contract)
    {
        return _toDomainConverters.ToDomain<TContract, TDomain>(contract);
    }

    public TContract ToContract<TDomain, TContract>(TDomain domain)
    {
        return _toContractConverters.ToContract<TDomain, TContract>(domain);
    }

    public IEnumerable<TContract> ToContract<TDomain, TContract>(IEnumerable<TDomain> domain)
    {
        return _toContractConverters.ToContract<TDomain, TContract>(domain);
    }
}