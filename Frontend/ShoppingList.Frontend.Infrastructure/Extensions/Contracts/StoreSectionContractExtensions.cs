using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreSectionContractExtensions
    {
        public static StoreSection ToModel(this StoreSectionContract contract)
        {
            return new StoreSection(contract.Id, contract.Name, contract.SortingIndex, contract.IsDefautlSection);
        }
    }
}
