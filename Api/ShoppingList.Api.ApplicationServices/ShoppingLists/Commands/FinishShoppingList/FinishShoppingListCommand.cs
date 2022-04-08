using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.FinishShoppingList;

public class FinishShoppingListCommand : ICommand<bool>
{
    public FinishShoppingListCommand(ShoppingListId shoppingListId, DateTimeOffset completionDate)
    {
        ShoppingListId = shoppingListId;
        CompletionDate = completionDate;
    }

    public ShoppingListId ShoppingListId { get; }
    public DateTimeOffset CompletionDate { get; }
}