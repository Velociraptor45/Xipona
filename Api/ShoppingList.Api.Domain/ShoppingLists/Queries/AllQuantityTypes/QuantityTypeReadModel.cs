﻿namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;

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

    public int Id { get; }
    public string Name { get; }
    public int DefaultQuantity { get; }
    public string PriceLabel { get; }
    public string QuantityLabel { get; }
    public int QuantityNormalizer { get; }
}