namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityInPacketTypeContract
    {
        public QuantityInPacketTypeContract(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}