using Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.RecipeTags.Models;
using Xipona.Api.Domain.TestKit.RecipeTags.Services.Creation;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.RecipeTags.Commands;

public class CreateRecipeTagCommandHandlerTests : CommandHandlerTestsBase<CreateRecipeTagCommandHandler,
    CreateRecipeTagCommand, IRecipeTag>
{
    public CreateRecipeTagCommandHandlerTests() : base(new CreateRecipeTagCommandHandlerFixture())
    {
    }

    private sealed class CreateRecipeTagCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly RecipeTagCreationServiceMock _recipeTagCreationServiceMock = new(MockBehavior.Strict);

        public override CreateRecipeTagCommand? Command { get; protected set; }
        public override IRecipeTag? ExpectedResult { get; protected set; }

        public override CreateRecipeTagCommandHandler CreateSut()
        {
            return new(_ => _recipeTagCreationServiceMock.Object, TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupExpectedResult();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _recipeTagCreationServiceMock.VerifyCreateAsync(Command.Name, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateRecipeTagCommand>().Create();
        }

        private void SetupExpectedResult()
        {
            ExpectedResult = new RecipeTagBuilder().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            _recipeTagCreationServiceMock.SetupCreateAsync(Command.Name, ExpectedResult);
        }
    }
}