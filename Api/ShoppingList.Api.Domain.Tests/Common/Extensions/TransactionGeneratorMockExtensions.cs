using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class TransactionGeneratorMockExtensions
    {
        public static void SetupGenerateAsync(this Mock<ITransactionGenerator> mock, ITransaction returnValue)
        {
            mock
                .Setup(i => i.GenerateAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }
    }
}