using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    public class ItemAmountsForOneServingContract
    {
        public ItemAmountsForOneServingContract(IEnumerable<ItemAmountForOneServingContract> items)
        {
            Items = items;
        }

        public IEnumerable<ItemAmountForOneServingContract> Items { get; }
    }
}