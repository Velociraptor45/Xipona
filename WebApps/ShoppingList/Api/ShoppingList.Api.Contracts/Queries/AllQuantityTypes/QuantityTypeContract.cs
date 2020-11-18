namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityTypeContract
    {
        public QuantityTypeContract(int id, string name, int defaultQuantity, string label)
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