namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class IngredientQuantityType
{
    public IngredientQuantityType(int id, string quantityLabel)
    {
        Id = id;
        QuantityLabel = quantityLabel;
    }

    public int Id { get; set; }
    public string QuantityLabel { get; set; }
}