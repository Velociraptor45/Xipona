namespace ShoppingList.Api.Domain.Queries.AllQuantityTypes
{
    public class QuantityTypeReadModel
    {
        public QuantityTypeReadModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}