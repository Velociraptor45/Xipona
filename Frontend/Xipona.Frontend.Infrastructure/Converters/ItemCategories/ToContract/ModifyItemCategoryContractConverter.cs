using Xipona.Api.Contracts.ItemCategories.Commands;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ItemCategories;

namespace Xipona.Frontend.Infrastructure.Converters.ItemCategories.ToContract;

public class ModifyItemCategoryContractConverter :
    IToContractConverter<ModifyItemCategoryRequest, ModifyItemCategoryContract>
{
    public ModifyItemCategoryContract ToContract(ModifyItemCategoryRequest source)
    {
        return new ModifyItemCategoryContract(source.ItemCategoryId, source.Name);
    }
}