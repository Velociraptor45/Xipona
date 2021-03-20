namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class Manufacturer
    {
        public Manufacturer(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}