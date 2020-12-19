using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories
{
    public class StoreFactory : IStoreFactory
    {
        public IStore Create(StoreId id, string name, bool isDeleted)
        {
            return new Store(id, name, isDeleted);
        }
    }
}