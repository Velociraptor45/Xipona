namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

public class QuantityTypeInPacketReadModel
{
    public QuantityTypeInPacketReadModel(int id, string name, string quantityLabel)
    {
        Id = id;
        Name = name;
        QuantityLabel = quantityLabel;
    }

    public int Id { get; }
    public string Name { get; }
    public string QuantityLabel { get; }
}