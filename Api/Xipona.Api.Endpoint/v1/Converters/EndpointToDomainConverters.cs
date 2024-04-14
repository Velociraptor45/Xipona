using ProjectHermes.Xipona.Api.Core.Converter;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters;

public class EndpointToDomainConverters : Dictionary<(Type, Type), IToDomainConverter>
{
    public IEnumerable<TDomain> ToDomain<TContract, TDomain>(IEnumerable<TContract> contract)
    {
        var typedConverter = GetConverter<TContract, TDomain>();

        return typedConverter.ToDomain(contract);
    }

    public TDomain ToDomain<TContract, TDomain>(TContract contract)
    {
        var typedConverter = GetConverter<TContract, TDomain>();

        return typedConverter.ToDomain(contract);
    }

    private IToDomainConverter<TContract, TDomain> GetConverter<TContract, TDomain>()
    {
        var domainType = typeof(TContract);
        var contractType = typeof(TDomain);

        if (!TryGetValue((domainType, contractType), out var converter))
        {
            throw new InvalidOperationException($"No converter for {contractType} to {domainType} found.");
        }

        var typedConverter = converter as IToDomainConverter<TContract, TDomain>;
        if (typedConverter == null)
            throw new InvalidOperationException($"Registered converter for {contractType} to {domainType} does not convert said types.");

        return typedConverter;
    }
}