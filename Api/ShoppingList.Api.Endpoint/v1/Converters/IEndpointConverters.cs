using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

public interface IEndpointConverters
{
    TContract ToContract<TDomain, TContract>(TDomain domain);
    IEnumerable<TContract> ToContract<TDomain, TContract>(IEnumerable<TDomain> domain);
    TDomain ToDomain<TContract, TDomain>(TContract contract);
    IEnumerable<TDomain> ToDomain<TContract, TDomain>(IEnumerable<TContract> contract);
}