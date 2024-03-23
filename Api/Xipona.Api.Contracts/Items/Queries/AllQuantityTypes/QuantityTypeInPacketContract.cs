namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes
{
    public class QuantityTypeInPacketContract
    {
        public QuantityTypeInPacketContract(int id, string name, string quantityLabel)
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