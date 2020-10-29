﻿namespace ShoppingList.Api.Contracts.Queries.AllQuantityTypes
{
    public class QuantityTypesContract
    {
        public QuantityTypesContract(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}