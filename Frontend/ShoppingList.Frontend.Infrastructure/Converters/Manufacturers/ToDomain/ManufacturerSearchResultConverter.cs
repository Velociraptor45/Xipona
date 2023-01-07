using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Manufacturers.ToDomain
{
    public class ManufacturerSearchResultConverter :
        IToDomainConverter<ManufacturerSearchResultContract, ManufacturerSearchResult>
    {
        public ManufacturerSearchResult ToDomain(ManufacturerSearchResultContract source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new ManufacturerSearchResult(source.Id, source.Name);
        }
    }
}