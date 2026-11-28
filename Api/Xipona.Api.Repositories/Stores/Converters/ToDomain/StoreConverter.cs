using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Models.Factories;
using Section = Xipona.Api.Repositories.Stores.Entities.Section;

namespace Xipona.Api.Repositories.Stores.Converters.ToDomain;

public class StoreConverter : IToDomainConverter<Entities.Store, IStore>
{
    private readonly IStoreFactory _storeFactory;
    private readonly IToDomainConverter<Section, ISection> _sectionConverter;

    public StoreConverter(IStoreFactory storeFactory,
        IToDomainConverter<Section, ISection> sectionConverter)
    {
        _storeFactory = storeFactory;
        _sectionConverter = sectionConverter;
    }

    public IStore ToDomain(Entities.Store source)
    {
        List<ISection> sections = _sectionConverter.ToDomain(source.Sections).ToList();

        var store = (AggregateRoot)_storeFactory.Create(
            new StoreId(source.Id),
            new StoreName(source.Name),
            source.Deleted,
            sections,
            source.CreatedAt);

        store.EnrichWithRowVersion(source.RowVersion);
        return (store as IStore)!;
    }
}