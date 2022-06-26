using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.ItemCategories.Models
{
    public class ItemCategoriesState
    {
        private List<ItemCategorySearchResult> _searchResults;

        public ItemCategoriesState()
        {
            _searchResults = new List<ItemCategorySearchResult>();
        }

        public IReadOnlyCollection<ItemCategorySearchResult> SearchResults => _searchResults;
        public ItemCategory EditedItemCategory { get; set; }

        public void RegisterSearchResults(IEnumerable<ItemCategorySearchResult> searchResults)
        {
            _searchResults = searchResults.ToList();
        }

        public void RemoveFromSearchResultsIfExists(Guid itemCategoryId)
        {
            var itemCategory = _searchResults.FirstOrDefault(r => r.Id == itemCategoryId);
            if (itemCategory is null)
                return;

            _searchResults.Remove(itemCategory);
        }

        public void UpdateItemCategorySearchResultName(Guid itemCategoryId, string name)
        {
            var itemCategory = _searchResults.FirstOrDefault(r => r.Id == itemCategoryId);
            if (itemCategory is null)
                return;

            itemCategory.ChangeName(name);
        }

        public void SetEditedItemCategory(ItemCategory itemCategory)
        {
            EditedItemCategory = itemCategory;
        }

        public void SetNewEditedItemCategory()
        {
            SetEditedItemCategory(new ItemCategory(Guid.Empty, "New ItemCategory"));
        }

        public void ResetEditedItemCategory()
        {
            SetEditedItemCategory(null);
        }
    }
}