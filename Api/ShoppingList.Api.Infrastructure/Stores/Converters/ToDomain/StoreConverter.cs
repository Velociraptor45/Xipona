using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToDomain;

public class StoreConverter : IToDomainConverter<Entities.Store, IStore>
{
    private readonly IStoreFactory storeFactory;
    private readonly IToDomainConverter<Section, IStoreSection> storeSectionConverter;

    public StoreConverter(IStoreFactory storeFactory,
        IToDomainConverter<Section, IStoreSection> storeSectionConverter)
    {
        this.storeFactory = storeFactory;
        this.storeSectionConverter = storeSectionConverter;
    }

    public IStore ToDomain(Entities.Store source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        List<IStoreSection> sections = storeSectionConverter.ToDomain(source.Sections).ToList();

        return storeFactory.Create(
            new StoreId(source.Id),
            source.Name,
            source.Deleted,
            sections);
    }
}