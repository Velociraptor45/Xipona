namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;

public interface ICommandHandler<TCommand, TValue>
    where TCommand : ICommand<TValue>
{
    Task<TValue> HandleAsync(TCommand command, CancellationToken cancellationToken);
}