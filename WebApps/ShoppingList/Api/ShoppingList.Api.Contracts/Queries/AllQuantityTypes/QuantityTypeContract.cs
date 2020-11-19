namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityTypeContract
    {
        public QuantityTypeContract(int id, string name, int defaultQuantity, string pricelabel)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Pricelabel = pricelabel;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public string Pricelabel { get; }
    }
}