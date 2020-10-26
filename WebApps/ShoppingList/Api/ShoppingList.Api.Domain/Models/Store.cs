namespace ShoppingList.Api.Domain.Models
{
    public class Store
    {
        public Store(StoreId id, string name, bool isDeleted)
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