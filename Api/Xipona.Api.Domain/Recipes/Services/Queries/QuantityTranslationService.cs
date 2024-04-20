﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

public class QuantityTranslationService : IQuantityTranslationService
{
    private const float _mlInTablespoon = 18f;
    private const float _mlInTeaspoon = _mlInTablespoon / 3;
    private const int _unitToWeightFactor = 100;

    public (QuantityType QuantityType, Quantity Quantity) NormalizeForOneServing(NumberOfServings definedNumberOfServings,
        IngredientQuantityType ingredientQuantityType, IngredientQuantity ingredientQuantity, ItemQuantity itemQuantity)
    {
        switch (ingredientQuantityType)
        {
            case IngredientQuantityType.Unit:
                var quantityPerServingUnit = ingredientQuantity.Value / definedNumberOfServings;
                if (itemQuantity.Type == QuantityType.Weight)
                {
                    return (QuantityType.Weight, new Quantity(quantityPerServingUnit * _unitToWeightFactor));
                }

                if (itemQuantity.InPacket!.Type == QuantityTypeInPacket.Unit)
                {
                    return (QuantityType.Unit, new Quantity(quantityPerServingUnit / itemQuantity.InPacket.Quantity));
                }

                return (QuantityType.Unit, new Quantity(ingredientQuantity.Value / definedNumberOfServings));
            case IngredientQuantityType.Weight:
                if (itemQuantity.Type == QuantityType.Weight)
                    return (QuantityType.Weight, new Quantity(ingredientQuantity.Value / definedNumberOfServings));

                var quantityPerServingWeight = ingredientQuantity.Value / definedNumberOfServings;
                return (QuantityType.Unit, new Quantity(quantityPerServingWeight / itemQuantity.InPacket!.Quantity));
            case IngredientQuantityType.Fluid:
                if (itemQuantity.Type == QuantityType.Weight)
                    return (QuantityType.Weight, new Quantity(ingredientQuantity.Value / definedNumberOfServings));

                var quantityPerServingFluid = ingredientQuantity.Value / definedNumberOfServings;
                return (QuantityType.Unit, new Quantity(quantityPerServingFluid / itemQuantity.InPacket!.Quantity));
            case IngredientQuantityType.Tablespoon:
                return NormalizeForSpoon(definedNumberOfServings, ingredientQuantity, itemQuantity, _mlInTablespoon);
            case IngredientQuantityType.Teaspoon:
                return NormalizeForSpoon(definedNumberOfServings, ingredientQuantity, itemQuantity, _mlInTeaspoon);
            default:
                throw new ArgumentOutOfRangeException(nameof(ingredientQuantityType), ingredientQuantityType, null);
        }
    }

    private static (QuantityType, Quantity) NormalizeForSpoon(NumberOfServings numberOfServings,
        IngredientQuantity ingredientQuantity, ItemQuantity itemQuantity, float spoonCapacity)
    {
        if (itemQuantity.Type == QuantityType.Weight)
            return (QuantityType.Weight, new Quantity(spoonCapacity * ingredientQuantity.Value / numberOfServings));

        var unitsPerSpoon = spoonCapacity / itemQuantity.InPacket!.Quantity;
        var unitsForRecipe = unitsPerSpoon * ingredientQuantity.Value;
        return (QuantityType.Unit, new Quantity(unitsForRecipe / numberOfServings));
    }
}