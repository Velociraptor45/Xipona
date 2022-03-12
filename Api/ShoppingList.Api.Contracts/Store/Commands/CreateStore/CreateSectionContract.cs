namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore
{
    public class CreateSectionContract
    {
        public string Name { get; set; }
        public int SortingIndex { get; set; }
        public bool IsDefaultSection { get; set; }
    }
}