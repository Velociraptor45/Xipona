using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

public class QuantityTypeInPacketReadModel
{
    public QuantityTypeInPacketReadModel(int id, string name, string quantityLabel)
    {
        Id = id;
        Name = name;
        QuantityLabel = quantityLabel;
    }

    public QuantityTypeInPacketReadModel(QuantityTypeInPacket quantityTypeInPacket) :
        this(
            (int)quantityTypeInPacket,
            quantityTypeInPacket.ToString(),
            quantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel)
    {
    }

    public int Id { get; }
    public string Name { get; }
    public string QuantityLabel { get; }
}