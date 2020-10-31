namespace ShoppingList.Api.Contracts.Queries.AllActiveManufacturers
{
    public class ActiveManufacturerContract
    {
        public ActiveManufacturerContract(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}