using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityInPacketTypes;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class QuantityTypeInPacketExtensions
    {
        public static QuantityInPacketTypeReadModel ToReadModel(this QuantityTypeInPacket quantityTypeInPacket)
        {
            return new QuantityInPacketTypeReadModel(
                (int)quantityTypeInPacket,
                quantityTypeInPacket.ToString(),
                quantityTypeInPacket.GetAttribute<QuantityLabelAttribute>().QuantityLabel);
        }
    }
}