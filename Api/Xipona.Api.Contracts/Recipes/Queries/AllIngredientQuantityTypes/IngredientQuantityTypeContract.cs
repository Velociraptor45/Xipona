namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes
{
    /// <summary>
    /// Represents the contract for an ingredient quantity type.
    /// </summary>
    public class IngredientQuantityTypeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantityLabel"></param>
        public IngredientQuantityTypeContract(int id, string quantityLabel)
        {
            Id = id;
            QuantityLabel = quantityLabel;
        }

        /// <summary>
        /// The ID of the quantity type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The label of the quantity type.
        /// </summary>
        public string QuantityLabel { get; set; }
    }
}