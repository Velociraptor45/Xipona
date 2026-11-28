using Xipona.Api.Core.Attributes;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Recipes.Models;

namespace Xipona.Api.Domain.Recipes.Services.Queries.Quantities;

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