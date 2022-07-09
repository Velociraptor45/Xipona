using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Stores;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToContract
{
    public class UpdateStoreContractConverter : IToContractConverter<ModifyStoreRequest, UpdateStoreContract>
    {
        public UpdateStoreContract ToContract(ModifyStoreRequest request)
        {
            var sections = request.Sections.Select(s => new UpdateSectionContract(
                s.Id.BackendId == Guid.Empty ? null : s.Id.BackendId,
                s.Name,
                s.SortingIndex,
                s.IsDefaultSection));

            return new UpdateStoreContract(
                request.StoreId,
                request.Name,
                sections);
        }
    }
}