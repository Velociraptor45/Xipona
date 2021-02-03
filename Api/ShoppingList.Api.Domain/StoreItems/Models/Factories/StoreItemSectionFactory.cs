using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemSectionFactory : IStoreItemSectionFactory
    {
        public IStoreItemSection Create(StoreItemSectionId id, string name, int sortIndex)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespaces", nameof(name));
            }

            return new StoreItemSection(id, name, sortIndex);
        }
    }
}