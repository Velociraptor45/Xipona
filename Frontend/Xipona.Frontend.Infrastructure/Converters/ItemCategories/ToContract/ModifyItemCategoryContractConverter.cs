using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ItemCategories.ToContract;

public class ModifyItemCategoryContractConverter :
    IToContractConverter<ModifyItemCategoryRequest, ModifyItemCategoryContract>
{
    public ModifyItemCategoryContract ToContract(ModifyItemCategoryRequest source)
    {
        return new ModifyItemCategoryContract(source.ItemCategoryId, source.Name);
    }
}