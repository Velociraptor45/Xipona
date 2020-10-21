using ShoppingList.Domain.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.ApplicationServices
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken)
        {
            var valueType = query.GetType()
                .GetInterfaces()
                .FirstOrDefault(interf => interf.IsGenericType && interf.GetGenericTypeDefinition() == typeof(IQuery<>)
                .GetGenericArguments().First());

            var serviceType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), valueType);
            var service = serviceProvider.GetService(valueType);
            var method = serviceType.GetMethod("HandlAsync");

            if (!(method.Invoke(service, new object[] { query, cancellationToken }) is Task<T> task))
            {
                throw new InvalidOperationException();
            }

            return task;
        }
    }
}