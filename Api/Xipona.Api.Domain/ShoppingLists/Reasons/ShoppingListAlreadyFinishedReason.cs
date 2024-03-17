using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;

public class ShoppingListAlreadyFinishedReason : IReason
{
    public ShoppingListAlreadyFinishedReason(ShoppingListId id)
    {
        Message = $"Shopping list {id.Value} is already finished.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.ShoppingListAlreadyFinished;
}