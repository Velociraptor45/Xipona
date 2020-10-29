namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityTypeContract
    {
        public QuantityTypeContract(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}