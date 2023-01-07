using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public interface IItemsApiService
    {
        Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemsByItemCategoryAsync(Guid itemCategoryId);
    }
}