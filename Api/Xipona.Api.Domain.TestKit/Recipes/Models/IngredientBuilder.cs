using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;

public class IngredientBuilder : DomainTestBuilderBase<Ingredient>
{
    public IngredientBuilder()
    {
    }

    public IngredientBuilder(Ingredient ingredient)
    {
        WithId(ingredient.Id);
        WithItemCategoryId(ingredient.ItemCategoryId);
        WithQuantityType(ingredient.QuantityType);
        WithQuantity(ingredient.Quantity);
        WithShoppingListProperties(ingredient.ShoppingListProperties);
    }

    public IngredientBuilder WithId(IngredientId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public IngredientBuilder WithItemCategoryId(ItemCategoryId itemCategoryId)
    {
        FillConstructorWith(nameof(itemCategoryId), itemCategoryId);
        return this;
    }

    public IngredientBuilder WithQuantityType(IngredientQuantityType quantityType)
    {
        FillConstructorWith(nameof(quantityType), quantityType);
        return this;
    }

    public IngredientBuilder WithQuantity(IngredientQuantity quantity)
    {
        FillConstructorWith(nameof(quantity), quantity);
        return this;
    }

    public IngredientBuilder WithShoppingListProperties(IngredientShoppingListProperties? shoppingListProperties)
    {
        FillConstructorWith(nameof(shoppingListProperties), shoppingListProperties);
        return this;
    }

    public IngredientBuilder WithoutShoppingListProperties()
    {
        return WithShoppingListProperties(null);
    }
}