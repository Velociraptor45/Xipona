using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToContract
{
    public class UpdateStoreContractConverter : IToContractConverter<ModifyStoreRequest, UpdateStoreContract>
    {
        public UpdateStoreContract ToContract(ModifyStoreRequest request)
        {
            var sections = request.Sections.Select(s => new UpdateSectionContract
            {
                Id = s.Id.BackendId == Guid.Empty ? null : s.Id.BackendId,
                Name = s.Name,
                IsDefaultSection = s.IsDefaultSection,
                SortingIndex = s.SortingIndex
            });

            return new UpdateStoreContract()
            {
                Id = request.StoreId,
                Name = request.Name,
                Sections = sections
            };
        }
    }
}