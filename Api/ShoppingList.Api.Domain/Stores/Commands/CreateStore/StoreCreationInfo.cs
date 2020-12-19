using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore
{
    public class StoreCreationInfo
    {
        public StoreCreationInfo(StoreId id, string name)
        {
            Id = id;
            Name = name;
        }

        public StoreId Id { get; }
        public string Name { get; }
    }
}