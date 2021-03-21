using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class StoreSectionExtensions
    {
        public static StoreSectionContract ToContract(this StoreSection section)
        {
            return new StoreSectionContract
            {
                Id = section.Id.BackendId,
                Name = section.Name,
                SortingIndex = section.SortingIndex,
                IsDefaultSection = section.IsDefaultSection
            };
        }
    }
}