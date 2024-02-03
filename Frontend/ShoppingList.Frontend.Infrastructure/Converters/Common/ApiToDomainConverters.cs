using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common
{
    public class ApiToDomainConverters : Dictionary<(Type, Type), IToDomainConverter>
    {
        public IEnumerable<TDomain> ToDomain<TContract, TDomain>(IEnumerable<TContract> contract)
        {
            if (contract is null)
                throw new ArgumentNullException(nameof(contract));

            var typedConverter = GetConverter<TContract, TDomain>();

            return typedConverter.ToDomain(contract);
        }

        public TDomain ToDomain<TContract, TDomain>(TContract contract)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

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
}