using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemSectionReadModel
    {
        public StoreItemSectionReadModel(StoreItemSectionId id, string name, int sortingIndex)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
        }

        public StoreItemSectionId Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
    }
}