﻿using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Api.Domain.Commands.UpdateItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Converters.Item
{
    public static class UpdateItemContractExtensions
    {
        public static ItemUpdate ToItemUpdate(this UpdateItemContract contract)
        {
            return new ItemUpdate(new StoreItemId(contract.OldId), contract.Name, contract.Comment,
                (QuantityType)contract.QuantityType, contract.QuantityInPacket,
                (QuantityTypeInPacket)contract.QuantityTypeInPacket,
                new ItemCategoryId(contract.ItemCategoryId), new ManufacturerId(contract.ManufacturerId),
                contract.Availabilities.Select(av => av.ToDomain()));
        }
    }
}