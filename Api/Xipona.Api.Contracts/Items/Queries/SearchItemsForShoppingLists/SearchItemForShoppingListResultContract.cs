using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists
{
    /// <summary>
    /// Represents a search result for an item or item type for a shopping list.
    /// </summary>
    public class SearchItemForShoppingListResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeId"></param>
        /// <param name="name"></param>
        /// <param name="defaultQuantity"></param>
        /// <param name="price"></param>
        /// <param name="priceLabel"></param>
        /// <param name="itemCategoryName"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="defaultSection"></param>
        public SearchItemForShoppingListResultContract(Guid id, Guid? typeId, string name, int defaultQuantity,
            decimal price, string priceLabel, string itemCategoryName, string manufacturerName,
            SectionContract defaultSection)
        {
            Id = id;
            TypeId = typeId;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            PriceLabel = priceLabel;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
            DefaultSection = defaultSection;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The ID of the item type. Null if the search result is a full item, not an item type.
        /// </summary>
        public Guid? TypeId { get; }

        /// <summary>
        /// The name of the item or item type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The default quantity of the item or item type.
        /// </summary>
        public int DefaultQuantity { get; }

        /// <summary>
        /// The price of the item or item type.
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The price label of the item or item type.
        /// </summary>
        public string PriceLabel { get; }

        /// <summary>
        /// The name of the item's category.
        /// </summary>
        public string ItemCategoryName { get; }

        /// <summary>
        /// The name of the item's manufacturer.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// The store's section where the item or item type is normally located.
        /// </summary>
        public SectionContract DefaultSection { get; }
    }
}