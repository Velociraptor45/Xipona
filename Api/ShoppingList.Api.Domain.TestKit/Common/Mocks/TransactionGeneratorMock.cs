using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.Common.Mocks
{
    public class TransactionGeneratorMock
    {
        private readonly Mock<ITransactionGenerator> mock;

        public TransactionGeneratorMock(Mock<ITransactionGenerator> mock)
        {
            this.mock = mock;
        }

        public TransactionGeneratorMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<ITransactionGenerator>>();
        }

        public void SetupGenerateAsync(ITransaction returnValue)
        {
            mock
                .Setup(i => i.GenerateAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void VerifyGenerateAsyncOnce()
        {
            mock.Verify(i => i.GenerateAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}