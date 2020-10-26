namespace ShoppingList.Frontend.Models
{
    public class Store
    {
        public Store(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}