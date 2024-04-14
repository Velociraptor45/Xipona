﻿using ProjectHermes.Xipona.Api.Core.Attributes;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;

public class QuantityTypeReadModel
{
    public QuantityTypeReadModel(int id, string name, int defaultQuantity, string priceLabel, string quantityLabel,
        int quantityNormalizer)
    {
        Id = id;
        Name = name;
        DefaultQuantity = defaultQuantity;
        PriceLabel = priceLabel;
        QuantityLabel = quantityLabel;
        QuantityNormalizer = quantityNormalizer;
    }

    public QuantityTypeReadModel(QuantityType quantityType) :
        this(
            (int)quantityType,
            quantityType.ToString(),
            quantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
            quantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
            quantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel,
            quantityType.GetAttribute<QuantityNormalizerAttribute>().Value)
    {
    }

    public int Id { get; }
    public string Name { get; }
    public int DefaultQuantity { get; }
    public string PriceLabel { get; }
    public string QuantityLabel { get; }
    public int QuantityNormalizer { get; }
}