namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemSectionFactory
    {
        IStoreItemSection Create(StoreItemSectionId id, string name, int sortIndex);
    }
}