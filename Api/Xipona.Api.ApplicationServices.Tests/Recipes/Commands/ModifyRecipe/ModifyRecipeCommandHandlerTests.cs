using Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.Recipes.Services.Modifications;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.Recipes.Commands.ModifyRecipe;

public class ModifyRecipeCommandHandlerTests : CommandHandlerTestsBase<
    ModifyRecipeCommandHandler, ModifyRecipeCommand, bool>
{
    public ModifyRecipeCommandHandlerTests() : base(new ModifyRecipeCommandHandlerFixture())
    {
    }

    private sealed class ModifyRecipeCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly RecipeModificationServiceMock _serviceMock = new(MockBehavior.Strict);

        public override ModifyRecipeCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override ModifyRecipeCommandHandler CreateSut()
        {
            return new ModifyRecipeCommandHandler(_ => _serviceMock.Object, TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _serviceMock.VerifyModifyAsync(Command.Modification, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyRecipeCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupModifyAsync(Command.Modification);
        }
    }
}