using Xipona.Api.Core.Attributes;

namespace Xipona.Api.Domain.Recipes.Models;

public enum IngredientQuantityType
{
    [QuantityLabel("x")]
    Unit = 0,

    [QuantityLabel("g")]
    Weight = 1,

    [QuantityLabel("ml")]
    Fluid = 2,

    [QuantityLabel("T")]
    Tablespoon,

    [QuantityLabel("tsp")]
    Teaspoon
}