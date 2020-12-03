namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityInPacketTypes
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