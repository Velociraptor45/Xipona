using FsCheck;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Tests.Experimental.TestKit.StoreItems.Models.StoreItems
{
    public static class StoreItemArbitrary
    {
        public static Arbitrary<StoreItem> GetArbitrary()
        {
            return (from id in Arb.Default.PositiveInt().Generator.Select(i => new ItemId(i.Get))
                    from name in Arb.Default.String().Generator
                    from isDeleted in Arb.Default.Bool().Generator
                    from comment in Arb.Default.String().Generator
                    from isTmp in Arb.Default.Bool().Generator
                    from quantType in Arb.From<QuantityType>().Generator
                    from quantInPacket in Arb.Default.Float32().Generator.Select(f => Math.Abs(f))
                    from quanTypeInPacket in Arb.From<QuantityTypeInPacket>().Generator
                    from itemCatId in Arb.Default.PositiveInt().Generator.Select(i => new ItemCategoryId(i.Get))
                    from manufId in Arb.Default.PositiveInt().Generator.Select(i => new ManufacturerId(i.Get))
                    from avails in Arb.From(StoreItemAvailabilityGenerator.GetGenerator()).Generator.ListOf()
                    from tmpId in Arb.Default.Guid().Generator.Select(i => new TemporaryItemId(i))
                    select new StoreItem(id, name, isDeleted, comment, isTmp, quantType, quantInPacket,
                        quanTypeInPacket, itemCatId, manufId, avails, tmpId)).ToArbitrary();
        }
    }
}
