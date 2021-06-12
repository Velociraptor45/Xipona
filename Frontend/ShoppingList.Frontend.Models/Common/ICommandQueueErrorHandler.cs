using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Common
{
    public interface ICommandQueueErrorHandler
    {
        void OnConnectionFailed();

        Task OnQueueProcessedAsync();

        void OnApiProcessingError();

        void Log(string content);
    }
}