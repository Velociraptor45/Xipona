namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes
{
    /// <summary>
    /// Represents a quantity type.
    /// </summary>
    public class QuantityTypeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="defaultQuantity"></param>
        /// <param name="priceLabel"></param>
        /// <param name="quantityLabel"></param>
        /// <param name="quantityNormalizer"></param>
        public QuantityTypeContract(int id, string name, int defaultQuantity, string priceLabel, string quantityLabel,
            int quantityNormalizer)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            PriceLabel = priceLabel;
            QuantityLabel = quantityLabel;
            QuantityNormalizer = quantityNormalizer;
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
        /// The default quantity of the quantity type.
        /// </summary>
        public int DefaultQuantity { get; }

        /// <summary>
        /// The label for the price of the quantity type.
        /// </summary>
        public string PriceLabel { get; }

        /// <summary>
        /// The label for the quantity type, e.g. g, ml, ...
        /// </summary>
        public string QuantityLabel { get; }

        /// <summary>
        /// The normalizer for the quantity type. Used to divide the quantity by this value to get the normalized quantity.
        /// The normalized quantity can be multiplied by the price.
        /// </summary>
        public int QuantityNormalizer { get; }
    }
}