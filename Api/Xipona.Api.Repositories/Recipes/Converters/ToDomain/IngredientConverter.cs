using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using Ingredient = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;

public class IngredientConverter : IToDomainConverter<Entities.Ingredient, IIngredient>
{
    private readonly IIngredientFactory _ingredientFactory;

    public IngredientConverter(Func<CancellationToken, IIngredientFactory> ingredientFactoryDelegate)
    {
        // TODO: find some kind of way to fix this and get a cancellation token into the ctor of converters (#239)
        _ingredientFactory = ingredientFactoryDelegate(default);
    }

    public IIngredient ToDomain(Ingredient source)
    {
        IngredientShoppingListProperties? properties = null;
        if (source.DefaultItemId is not null)
        {
            properties = new IngredientShoppingListProperties(
                new ItemId(source.DefaultItemId.Value),
                source.DefaultItemTypeId is null ? null : new ItemTypeId(source.DefaultItemTypeId.Value),
                new StoreId(source.DefaultStoreId!.Value),
                source.AddToShoppingListByDefault!.Value);
        }

        return _ingredientFactory.Create(
            new IngredientId(source.Id),
            new ItemCategoryId(source.ItemCategoryId),
            source.QuantityType.ToEnum<IngredientQuantityType>(),
            new IngredientQuantity(source.Quantity),
            properties);
    }
}