﻿using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class StoreItemSection
    {
        public StoreItemSection(Guid id, string name, int sortingIndex)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SortingIndex { get; }
    }
}