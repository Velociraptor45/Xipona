﻿using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ItemCategories.Commands.ModifyItemCategory;

public class ModifyItemCategoryCommandHandlerTests : CommandHandlerTestsBase<
    ModifyItemCategoryCommandHandler, ModifyItemCategoryCommand, bool>
{
    public ModifyItemCategoryCommandHandlerTests() : base(new ModifyItemCategoryCommandHandlerFixture())
    {
    }

    private sealed class ModifyItemCategoryCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ItemCategoryModificationServiceMock _itemCategoryModificationServiceMock =
            new(MockBehavior.Strict);

        public override ModifyItemCategoryCommand? Command { get; protected set; }

        public override bool ExpectedResult { get; protected set; } = true;

        public override ModifyItemCategoryCommandHandler CreateSut()
        {
            return new ModifyItemCategoryCommandHandler(TransactionGeneratorMock.Object,
                _ => _itemCategoryModificationServiceMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyItemCategoryCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _itemCategoryModificationServiceMock.SetupModifyAsync(Command.Modification);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _itemCategoryModificationServiceMock.VerifyModifyAsync(Command.Modification, Times.Once);
        }
    }
}