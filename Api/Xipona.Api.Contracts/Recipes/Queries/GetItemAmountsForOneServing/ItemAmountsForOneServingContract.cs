using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    /// <summary>
    /// Represents the normalized amount of items for one serving.
    /// </summary>
    public class ItemAmountsForOneServingContract
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        public ItemAmountsForOneServingContract(IEnumerable<ItemAmountForOneServingContract> items)
        {
            Items = items;
        }

        /// <summary>
        /// The normalized amount of items for one serving.
        /// </summary>
        public IEnumerable<ItemAmountForOneServingContract> Items { get; }
    }
}