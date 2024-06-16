using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Repositories.ItemCategories.Converters.ToContract;

public class ItemCategoryConverter : IToContractConverter<IItemCategory, Entities.ItemCategory>
{
    public Entities.ItemCategory ToContract(IItemCategory source)
    {
        return new Entities.ItemCategory
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}