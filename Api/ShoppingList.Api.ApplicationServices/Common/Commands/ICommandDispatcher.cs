namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;

public interface ICommandDispatcher
{
    Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken);
}