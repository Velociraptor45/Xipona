using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class ShoppingListStoreConverter : IToDomainConverter<Entities.Store, IShoppingListStore>
    {
        private readonly IShoppingListStoreFactory shoppingListStoreFactory;

        public ShoppingListStoreConverter(IShoppingListStoreFactory shoppingListStoreFactory)
        {
            this.shoppingListStoreFactory = shoppingListStoreFactory;
        }

        public IShoppingListStore ToDomain(Store source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return shoppingListStoreFactory.Create(
                new ShoppingListStoreId(source.Id),
                source.Name,
                source.Deleted);
        }
    }
}