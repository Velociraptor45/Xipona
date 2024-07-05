namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes
{
    /// <summary>
    /// Represents a quantity type.
    /// </summary>
    public class QuantityTypeInPacketContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="quantityLabel"></param>
        public QuantityTypeInPacketContract(int id, string name, string quantityLabel)
        {
            Id = id;
            Name = name;
            QuantityLabel = quantityLabel;
        }

        /// <summary>
        /// The ID of the quantity type.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name of the quantity type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The label for the quantity type, e.g. g, ml, ...
        /// </summary>
        public string QuantityLabel { get; }
    }
}