namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes
{
    public class QuantityInPacketTypeContract
    {
        public QuantityInPacketTypeContract(int id, string name, string quantityLabel)
        {
            Id = id;
            Name = name;
            QuantityLabel = quantityLabel;
        }

        public int Id { get; }
        public string Name { get; }
        public string QuantityLabel { get; }
    }
}