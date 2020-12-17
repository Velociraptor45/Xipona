using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories
{
    public interface IStoreFactory
    {
        IStore Create(StoreId id, string name, bool isDeleted);
    }
}