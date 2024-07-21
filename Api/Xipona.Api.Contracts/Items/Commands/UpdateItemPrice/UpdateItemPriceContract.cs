using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice
{
    public class UpdateItemPriceContract
    {
        public UpdateItemPriceContract(Guid? itemTypeId, Guid storeId, decimal price)
        {
            ItemTypeId = itemTypeId;
            StoreId = storeId;
            Price = price;
        }

        public Guid? ItemTypeId { get; set; }
        public Guid StoreId { get; set; }
        public decimal Price { get; set; }
    }
}