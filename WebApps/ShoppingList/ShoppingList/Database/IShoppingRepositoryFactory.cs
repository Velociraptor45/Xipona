namespace ShoppingList.Database
{
    public interface IShoppingRepositoryFactory
    {
        IShoppingRepository Create();
    }
}
