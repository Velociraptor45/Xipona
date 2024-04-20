using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Services.Creations;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandHandlerTests : CommandHandlerTestsBase<
    CreateRecipeCommandHandler, CreateRecipeCommand, RecipeReadModel>
{
    public CreateRecipeCommandHandlerTests() : base(new CreateRecipeCommandHandlerFixture())
    {
    }

    private sealed class CreateRecipeCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly RecipeCreationServiceMock _recipeCreationServiceMock =
            new RecipeCreationServiceMock(MockBehavior.Strict);

        public override CreateRecipeCommand? Command { get; protected set; }
        public override RecipeReadModel? ExpectedResult { get; protected set; }

        public override CreateRecipeCommandHandler CreateSut()
        {
            return new CreateRecipeCommandHandler(_ => _recipeCreationServiceMock.Object,
                TransactionGeneratorMock.Object);
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

            _recipeCreationServiceMock.VerifyCreateAsync(Command.Creation, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateRecipeCommand>().Create();
        }

        private void SetupExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<RecipeReadModel>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _recipeCreationServiceMock.SetupCreateAsync(Command.Creation, ExpectedResult);
        }
    }
}