namespace ProjectHermes.ShoppingList.Api.Contracts.Common.Queries
{
    public class StoreContract
    {
        public StoreContract(int id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}