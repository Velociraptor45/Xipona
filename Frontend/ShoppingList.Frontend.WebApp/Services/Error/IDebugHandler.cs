namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error
{
    public interface IDebugHandler
    {
        bool IsDebug { get; }

        void ToggleDebugState();
    }
}