﻿using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Converters.ToDomain;

public class RecipeConverter : IToDomainConverter<Entities.Recipe, IRecipe>
{
    private readonly IRecipeFactory _recipeFactory;
    private readonly IToDomainConverter<Entities.Ingredient, IIngredient> _ingredientConverter;
    private readonly IToDomainConverter<Entities.PreparationStep, IPreparationStep> _preparationStepConverter;

    public RecipeConverter(
        Func<CancellationToken, IRecipeFactory> recipeFactoryDelegate,
        IToDomainConverter<Entities.Ingredient, IIngredient> ingredientConverter,
        IToDomainConverter<Entities.PreparationStep, IPreparationStep> preparationStepConverter)
    {
        // TODO: find some kind of way to fix this and get a cancellation token into the ctor of converters (#239)
        _recipeFactory = recipeFactoryDelegate(default);
        _ingredientConverter = ingredientConverter;
        _preparationStepConverter = preparationStepConverter;
    }

    public IRecipe ToDomain(Entities.Recipe source)
    {
        var ingredients = _ingredientConverter.ToDomain(source.Ingredients);
        var steps = _preparationStepConverter.ToDomain(source.PreparationSteps);

        return _recipeFactory.Create(new RecipeId(source.Id), new RecipeName(source.Name), ingredients, steps);
    }
}