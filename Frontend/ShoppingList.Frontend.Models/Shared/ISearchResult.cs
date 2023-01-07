using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared;

public interface ISearchResult
{
    Guid Id { get; set; }
    string Name { get; set; }
}