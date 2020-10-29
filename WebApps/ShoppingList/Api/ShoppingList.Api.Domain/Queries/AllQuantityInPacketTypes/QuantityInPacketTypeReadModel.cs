namespace ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes
{
    public class QuantityInPacketTypeReadModel
    {
        public QuantityInPacketTypeReadModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}