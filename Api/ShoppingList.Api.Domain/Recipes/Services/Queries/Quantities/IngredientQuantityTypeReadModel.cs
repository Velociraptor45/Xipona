using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;

public class IngredientQuantityTypeReadModel
{
    public IngredientQuantityTypeReadModel(IngredientQuantityType quantityType)
        : this(quantityType.ToInt(), quantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel)
    {
    }

    public IngredientQuantityTypeReadModel(int id, string quantityLabel)
    {
        Id = id;
        QuantityLabel = quantityLabel;
    }

    public int Id { get; }
    public string QuantityLabel { get; }
}