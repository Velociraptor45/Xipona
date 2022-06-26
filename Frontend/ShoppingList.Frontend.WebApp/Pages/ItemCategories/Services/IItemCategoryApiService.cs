using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.ItemCategories.Services
{
    public interface IItemCategoryApiService
    {
        Task<IEnumerable<ItemCategorySearchResult>> SearchAsync(string searchInput);
        Task<ItemCategory> GetAsync(Guid itemCategoryId);
        Task<bool> DeleteAsync(Guid itemCategoryId);
        Task<ItemCategory> CreateAsync(ItemCategory itemCategory);
        Task<bool> ModifyAsync(ItemCategory itemCategory);
    }
}