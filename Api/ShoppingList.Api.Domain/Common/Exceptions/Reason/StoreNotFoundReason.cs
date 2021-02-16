using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using StoreModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class StoreNotFoundReason : IReason
    {
        public StoreNotFoundReason(StoreId id)
        {
            Message = $"Store {id.Value} not found.";
        }

        public StoreNotFoundReason(StoreModels.StoreId id)
        {
            Message = $"Store {id.Value} not found.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.StoreNotFound;
    }
}