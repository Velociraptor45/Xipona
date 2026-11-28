using Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.ItemCategories.Models;
using Xipona.Api.Domain.TestKit.ItemCategories.Services.Creations;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommandHandlerTests()
    : CommandHandlerTestsBase<CreateItemCategoryCommandHandler, CreateItemCategoryCommand, IItemCategory>
        (new CreateItemCategoryCommandHandlerFixture())
{
    private sealed class CreateItemCategoryCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ItemCategoryCreationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override CreateItemCategoryCommand? Command { get; protected set; }

        public override IItemCategory? ExpectedResult { get; protected set; } =
            ItemCategoryMother.NotDeleted().Create();

        public override CreateItemCategoryCommandHandler CreateSut()
        {
            return new(_ => _serviceMock.Object, TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.VerifyCreateAsync(Command.Name, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateItemCategoryCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            _serviceMock.SetupCreateAsync(Command.Name, ExpectedResult);
        }
    }
}