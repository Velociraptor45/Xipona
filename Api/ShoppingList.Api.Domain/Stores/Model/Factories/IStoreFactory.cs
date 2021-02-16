﻿using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories
{
    public interface IStoreFactory
    {
        IStore Create(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections);
    }
}