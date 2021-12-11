using ProjectHermes.ShoppingList.Api.Core.Converter;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters
{
    [Serializable]
    public class EndpointToDomainConverters : Dictionary<(Type, Type), IToDomainConverter>
    {
        public EndpointToDomainConverters()
        {
        }

        protected EndpointToDomainConverters(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public TDomain ToDomain<TContract, TDomain>(TContract contract)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            var domainType = contract.GetType();
            var contractType = typeof(TDomain);

            if (!TryGetValue((domainType, contractType), out var converter))
            {
                throw new InvalidOperationException($"No converter for {contractType} to {domainType} found.");
            }

            var typedConverter = converter as IToDomainConverter<TContract, TDomain>;
            if (typedConverter == null)
                throw new InvalidOperationException($"Registered converter for {contractType} to {domainType} does not convert said types.");

            return typedConverter.ToDomain(contract);
        }
    }
}