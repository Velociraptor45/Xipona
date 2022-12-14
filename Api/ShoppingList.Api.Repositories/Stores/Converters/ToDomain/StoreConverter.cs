using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToDomain;

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

        return _storeFactory.Create(
            new StoreId(source.Id),
            new StoreName(source.Name),
            source.Deleted,
            sections);
    }
}