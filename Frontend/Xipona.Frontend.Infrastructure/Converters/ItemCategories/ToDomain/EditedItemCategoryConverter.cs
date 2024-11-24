using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ItemCategories.ToDomain;

public class EditedItemCategoryConverter : IToDomainConverter<ItemCategoryContract, EditedItemCategory>
{
    public EditedItemCategory ToDomain(ItemCategoryContract source)
    {
        return new EditedItemCategory(source.Id, source.Name);
    }
}