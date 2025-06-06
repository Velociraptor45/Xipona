using Xipona.Api.Contracts.Common.Queries;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Infrastructure.Converters.ItemCategories.ToDomain;

public class EditedItemCategoryConverter : IToDomainConverter<ItemCategoryContract, EditedItemCategory>
{
    public EditedItemCategory ToDomain(ItemCategoryContract source)
    {
        return new EditedItemCategory(source.Id, source.Name);
    }
}