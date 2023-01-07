using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.ItemCategories.Services
{
    public interface IItemCategoryApiService
    {
        Task<ItemCategory> GetAsync(Guid itemCategoryId);

        Task<ItemCategory> CreateAsync(string itemCategoryName);
    }
}