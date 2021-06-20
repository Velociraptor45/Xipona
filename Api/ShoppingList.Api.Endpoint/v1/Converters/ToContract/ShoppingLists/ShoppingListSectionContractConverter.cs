using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists
{
    public class ShoppingListSectionContractConverter :
        IToContractConverter<ShoppingListSectionReadModel, ShoppingListSectionContract>
    {
        private readonly IToContractConverter<ShoppingListItemReadModel, ShoppingListItemContract> shoppingListItemContractConverter;

        public ShoppingListSectionContractConverter(
            IToContractConverter<ShoppingListItemReadModel, ShoppingListItemContract> shoppingListItemContractConverter)
        {
            this.shoppingListItemContractConverter = shoppingListItemContractConverter;
        }

        public ShoppingListSectionContract ToContract(ShoppingListSectionReadModel source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new ShoppingListSectionContract(
                source.Id.Value,
                source.Name,
                source.SortingIndex,
                source.IsDefaultSection,
                shoppingListItemContractConverter.ToContract(source.ItemReadModels));
        }
    }
}