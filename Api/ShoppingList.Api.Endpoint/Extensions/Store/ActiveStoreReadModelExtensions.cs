﻿using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class ActiveStoreReadModelExtensions
    {
        public static ActiveStoreContract ToContract(this ActiveStoreReadModel readModel)
        {
            return new ActiveStoreContract(readModel.Id.Value, readModel.Name, readModel.Items.Count);
        }
    }
}