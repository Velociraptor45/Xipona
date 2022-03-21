﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class QuantityTypeConverter : IToDomainConverter<QuantityTypeContract, QuantityType>
    {
        public QuantityType ToDomain(QuantityTypeContract source)
        {
            return new QuantityType(
                source.Id,
                source.Name,
                source.DefaultQuantity,
                source.Pricelabel,
                source.QuantityLabel,
                source.QuantityNormalizer);
        }
    }
}