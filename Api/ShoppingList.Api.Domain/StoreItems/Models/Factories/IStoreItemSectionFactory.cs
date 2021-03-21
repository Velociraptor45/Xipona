using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemSectionFactory
    {
        IStoreItemSection Create(StoreItemSectionId id, string name, int sortIndex);
        IStoreItemSection Create(IStoreSection section);
    }
}