using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore
{
    public class StoreUpdate
    {
        public StoreUpdate(StoreId id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
        }

        public StoreId Id { get; }
        public string Name { get; }
    }
}