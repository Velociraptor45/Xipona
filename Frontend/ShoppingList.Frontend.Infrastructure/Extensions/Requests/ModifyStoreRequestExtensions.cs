using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class ModifyStoreRequestExtensions
    {
        public static UpdateStoreContract ToContract(this ModifyStoreRequest request)
        {
            return new UpdateStoreContract()
            {
                Id = request.StoreId,
                Name = request.Name,
                Sections = request.Sections.Select(s =>
                    new StoreSectionContract
                    {
                        Id = s.Id,
                        Name = s.Name,
                        SortingIndex = s.SortingIndex,
                        IsDefaultSection = s.IsDefaultSection
                    })
            };
        }
    }
}