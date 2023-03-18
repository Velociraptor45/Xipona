using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ItemCategories.ToContract
{
    public class ModifyItemCategoryContractConverter :
        IToContractConverter<ModifyItemCategoryRequest, ModifyItemCategoryContract>
    {
        public ModifyItemCategoryContract ToContract(ModifyItemCategoryRequest source)
        {
            return new ModifyItemCategoryContract(source.ItemCategoryId, source.Name);
        }
    }
}