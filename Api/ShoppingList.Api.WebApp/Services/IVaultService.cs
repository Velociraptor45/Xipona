using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.WebApp.Services;

public interface IVaultService
{
    Task RegisterAsync(IServiceCollection services);
}