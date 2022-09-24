namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes
{
    public class IngredientQuantityTypeContract
    {
        public IngredientQuantityTypeContract(int id, string quantityLabel)
        {
            Id = id;
            QuantityLabel = quantityLabel;
        }

        public int Id { get; set; }
        public string QuantityLabel { get; set; }
    }
}