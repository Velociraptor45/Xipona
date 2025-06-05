using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Models.Factories;
using Xipona.Api.Domain.Stores.Models;
using Ingredient = Xipona.Api.Repositories.Recipes.Entities.Ingredient;

namespace Xipona.Api.Repositories.Recipes.Converters.ToDomain;

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