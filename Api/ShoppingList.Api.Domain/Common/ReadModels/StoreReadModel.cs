using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.ReadModels
{
    public class StoreReadModel
    {
        public StoreReadModel(StoreId id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public StoreId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}