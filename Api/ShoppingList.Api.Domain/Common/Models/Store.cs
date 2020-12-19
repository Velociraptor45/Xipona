namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public class Store : IStore
    {
        public Store(StoreId id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public StoreId Id { get; }
        public bool IsDeleted { get; }

        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}