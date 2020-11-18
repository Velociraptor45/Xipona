namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityInPacketTypeContract
    {
        public QuantityInPacketTypeContract(int id, string name, string priceLabel)
        {
            Id = id;
            Name = name;
            PriceLabel = priceLabel;
        }

        public int Id { get; }
        public string Name { get; }
        public string PriceLabel { get; }
    }
}