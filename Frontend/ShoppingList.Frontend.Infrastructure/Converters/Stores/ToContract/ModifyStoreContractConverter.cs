using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Stores;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToContract
{
    public class ModifyStoreContractConverter : IToContractConverter<ModifyStoreRequest, ModifyStoreContract>
    {
        public ModifyStoreContract ToContract(ModifyStoreRequest request)
        {
            var sections = request.Sections.Select(s => new ModifySectionContract(
                s.Id.BackendId == Guid.Empty ? null : s.Id.BackendId,
                s.Name,
                s.SortingIndex,
                s.IsDefaultSection));

            return new ModifyStoreContract(
                request.StoreId,
                request.Name,
                sections);
        }
    }
}