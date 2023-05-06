using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;

public class StoreConverter : IToContractConverter<IStore, Entities.Store>
{
    private readonly IToContractConverter<ISection, Section> _sectionConverter;

    public StoreConverter(IToContractConverter<ISection, Section> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public Entities.Store ToContract(IStore source)
    {
        return new Entities.Store()
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Sections = _sectionConverter.ToContract(source.Sections).ToList(),
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}