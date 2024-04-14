using System.Collections.Generic;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common
{
    public interface IApiConverters
    {
        TContract ToContract<TDomain, TContract>(TDomain domain);

        IEnumerable<TContract> ToContract<TDomain, TContract>(IEnumerable<TDomain> domain);

        TDomain ToDomain<TContract, TDomain>(TContract contract);

        IEnumerable<TDomain> ToDomain<TContract, TDomain>(IEnumerable<TContract> contract);
    }
}