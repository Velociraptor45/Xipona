using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToDomain;

public class StoreConverter : IToDomainConverter<Entities.Store, IStore>
{
    private readonly IStoreFactory _storeFactory;
    private readonly IToDomainConverter<Section, IStoreSection> _storeSectionConverter;

    public StoreConverter(IStoreFactory storeFactory,
        IToDomainConverter<Section, IStoreSection> storeSectionConverter)
    {
        _storeFactory = storeFactory;
        _storeSectionConverter = storeSectionConverter;
    }

    public IStore ToDomain(Entities.Store source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        List<IStoreSection> sections = _storeSectionConverter.ToDomain(source.Sections).ToList();

        return _storeFactory.Create(
            new StoreId(source.Id),
            source.Name,
            source.Deleted,
            sections);
    }
}