using Xipona.Api.Contracts.Manufacturers.Queries;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Manufacturers.States;
using System;

namespace Xipona.Frontend.Infrastructure.Converters.Manufacturers.ToDomain;

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