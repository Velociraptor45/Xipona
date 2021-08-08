using FsCheck;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Tests.Experimental.TestKit.StoreItems.Models.StoreItems
{
    public static class StoreItemAvailabilityGenerator
    {
        public static Gen<StoreItemAvailability> GetGenerator()
        {
            return (from storeId in Arb.Default.Int32().Generator
                    from price in Arb.Default.Float32().Generator
                    from sectionId in Arb.Default.Int32().Generator
                    select new StoreItemAvailability(new StoreId(storeId), price, new SectionId(sectionId)));
        }
    }
}
