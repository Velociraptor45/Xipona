using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using System;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Manufacturers.ToDomain
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