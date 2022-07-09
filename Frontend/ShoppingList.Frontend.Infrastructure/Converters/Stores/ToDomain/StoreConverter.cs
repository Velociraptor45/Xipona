using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToDomain
{
    public class StoreConverter : IToDomainConverter<ActiveStoreContract, Store>
    {
        public Store ToDomain(ActiveStoreContract contract)
        {
            var sections = contract.Sections.Select(s => new Section(
                new SectionId(s.Id, Guid.NewGuid()), s.Name, s.SortingIndex, s.IsDefaultSection));

            return new Store(contract.Id, contract.Name, sections);
        }
    }
}