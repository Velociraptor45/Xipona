namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes
{
    public class QuantityTypeContract
    {
        public QuantityTypeContract(int id, string name, int defaultQuantity, string pricelabel, string quantityLabel,
            int quantityNormalizer)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Pricelabel = pricelabel;
            QuantityLabel = quantityLabel;
            QuantityNormalizer = quantityNormalizer;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public string Pricelabel { get; }
        public string QuantityLabel { get; }
        public int QuantityNormalizer { get; }
    }
}