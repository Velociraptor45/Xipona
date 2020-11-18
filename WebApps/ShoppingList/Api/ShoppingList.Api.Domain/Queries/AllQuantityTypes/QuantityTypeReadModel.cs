namespace ShoppingList.Api.Domain.Queries.AllQuantityTypes
{
    public class QuantityTypeReadModel
    {
        public QuantityTypeReadModel(int id, string name, int defaultQuantity, string label)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Label = label;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public string Label { get; }
    }
}