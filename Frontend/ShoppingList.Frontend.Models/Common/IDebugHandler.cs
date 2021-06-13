namespace ProjectHermes.ShoppingList.Frontend.Models.Common
{
    public interface IDebugHandler
    {
        bool IsDebug { get; }

        void ToggleDebugState();
    }
}