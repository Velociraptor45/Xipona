using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using Ingredient = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Ingredient;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Converters.ToDomain;

public class IngredientConverter : IToDomainConverter<Ingredient, IIngredient>
{
    private readonly IIngredientFactory _ingredientFactory;

    public IngredientConverter(Func<CancellationToken, IIngredientFactory> ingredientFactoryDelegate)
    {
        // TODO: find some kind of way to fix this and get a cancellation token into the ctor of converters (#239)
        _ingredientFactory = ingredientFactoryDelegate(default);
    }

    public IIngredient ToDomain(Ingredient source)
    {
        return _ingredientFactory.Create(
            new IngredientId(source.Id),
            new ItemCategoryId(source.ItemCategoryId),
            source.QuantityType.ToEnum<IngredientQuantityType>(),
            new IngredientQuantity(source.Quantity),
            source.DefaultItemId is null ? null : new ItemId(source.DefaultItemId.Value),
            source.DefaultItemTypeId is null ? null : new ItemTypeId(source.DefaultItemTypeId.Value));
    }
}