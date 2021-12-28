using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.Common.Mocks
{
    public class TransactionGeneratorMock : Mock<ITransactionGenerator>
    {
        public TransactionGeneratorMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void SetupGenerateAsync(ITransaction returnValue)
        {
            Setup(i => i.GenerateAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void VerifyGenerateAsyncOnce()
        {
            Verify(i => i.GenerateAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}