using ShoppingList.Domain.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.ApplicationServices
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<T> DispatchAsync<T>(ICommand<T> query, CancellationToken cancellationToken)
        {
            var valueType = query.GetType()
                .GetInterfaces()
                .FirstOrDefault(interf => interf.IsGenericType && interf.GetGenericTypeDefinition() == typeof(ICommand<>))
                .GetGenericArguments()
                .First();

            var serviceType = typeof(ICommandHandler<,>).MakeGenericType(query.GetType(), valueType);
            var service = serviceProvider.GetService(serviceType);
            var method = serviceType.GetMethod("HandleAsync");

            if (!(method.Invoke(service, new object[] { query, cancellationToken }) is Task<T> task))
            {
                throw new InvalidOperationException();
            }

            return task;
        }
    }
}