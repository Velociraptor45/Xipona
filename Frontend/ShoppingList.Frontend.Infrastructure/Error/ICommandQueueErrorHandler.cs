using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error
{
    public interface ICommandQueueErrorHandler
    {
        void OnConnectionFailed();

        Task OnQueueProcessedAsync();

        void OnApiProcessingError();

        void Log(string content);
    }
}