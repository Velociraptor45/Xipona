using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.FinishShoppingList;

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