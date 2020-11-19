namespace ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes
{
    public class QuantityInPacketTypeReadModel
    {
        public QuantityInPacketTypeReadModel(int id, string name, string quantityLabel)
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