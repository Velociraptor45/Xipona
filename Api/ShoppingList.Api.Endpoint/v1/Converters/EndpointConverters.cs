using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

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
        if (contract == null)
            throw new ArgumentNullException(nameof(contract));

        return _toDomainConverters.ToDomain<TContract, TDomain>(contract);
    }

    public IEnumerable<TDomain> ToDomain<TContract, TDomain>(IEnumerable<TContract> contract)
    {
        if (contract == null)
            throw new ArgumentNullException(nameof(contract));

        return _toDomainConverters.ToDomain<TContract, TDomain>(contract);
    }

    public TContract ToContract<TDomain, TContract>(TDomain domain)
    {
        if (domain == null)
            throw new ArgumentNullException(nameof(domain));

        return _toContractConverters.ToContract<TDomain, TContract>(domain);
    }

    public IEnumerable<TContract> ToContract<TDomain, TContract>(IEnumerable<TDomain> domain)
    {
        if (domain == null)
            throw new ArgumentNullException(nameof(domain));

        return _toContractConverters.ToContract<TDomain, TContract>(domain);
    }
}