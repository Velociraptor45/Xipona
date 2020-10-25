namespace ShoppingList.Contracts.Queries.AllActiveStores
{
    public class ActiveStoreContract
    {
        public ActiveStoreContract(int id, string name, int itemCount)
        {
            Id = id;
            Name = name;
            ItemCount = itemCount;
        }

        public int Id { get; }
        public string Name { get; }
        public int ItemCount { get; }
    }
}