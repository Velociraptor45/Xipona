﻿using ShoppingList.Contracts.Queries;
using ShoppingList.Domain.Queries.ItemSearch;

namespace ShoppingList.Endpoint.Converters.Item
{
    public static class ItemSearchContractConverter
    {
        public static ItemSearchContract ToContract(this ItemSearchReadModel readModel)
        {
            return new ItemSearchContract(readModel.Id.Value, readModel.Name, readModel.Price,
                readModel.ItemCategory.Name, readModel.Manufacturer.Name);
        }
    }
}