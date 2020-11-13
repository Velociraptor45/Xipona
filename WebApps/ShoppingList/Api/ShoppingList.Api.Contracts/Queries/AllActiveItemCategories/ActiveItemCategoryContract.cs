namespace ShoppingList.Api.Contracts.Queries.AllActiveItemCategories
{
    public class ActiveItemCategoryContract
    {
        public ActiveItemCategoryContract(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}