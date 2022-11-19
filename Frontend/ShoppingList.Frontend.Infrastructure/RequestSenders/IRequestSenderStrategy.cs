using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

public interface IRequestSenderStrategy
{
    Task SendAsync(IApiRequest request);
}