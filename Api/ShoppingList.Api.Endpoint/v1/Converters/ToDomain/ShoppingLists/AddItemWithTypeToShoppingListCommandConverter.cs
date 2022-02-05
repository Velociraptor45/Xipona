using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class AddItemWithTypeToShoppingListCommandConverter
    : IToDomainConverter<AddItemWithTypeToShoppingListContract, AddItemWithTypeToShoppingListCommand>
{
    public AddItemWithTypeToShoppingListCommand ToDomain(AddItemWithTypeToShoppingListContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new AddItemWithTypeToShoppingListCommand(
            new ShoppingListId(source.ShoppingListId),
            new ItemId(source.ItemId),
            new ItemTypeId(source.ItemTypeId),
            source.SectionId.HasValue ? new SectionId(source.SectionId.Value) : null,
            source.Quantity);
    }
}