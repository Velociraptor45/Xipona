using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;

public class ApiConverters : IApiConverters
{
    private readonly ApiToDomainConverters _toDomainConverters;
    private readonly ApiToContractConverters _toContractConverters;

    public ApiConverters(ApiToDomainConverters toDomainConverters,
        ApiToContractConverters toContractConverters)
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