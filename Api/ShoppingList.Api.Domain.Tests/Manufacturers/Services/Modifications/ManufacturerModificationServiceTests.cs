using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Manufacturers.Services.Modifications;

public class ManufacturerModificationServiceTests
{
    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture;

        public ModifyAsync()
        {
            _fixture = new ModifyAsyncFixture();
        }

        [Fact]
        public async Task ModifyAsync_WithNotFindingManufacturer_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupManufacturerMock();
            _fixture.SetupManufacturerModification();
            _fixture.SetupNotFindingManufacturer();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ManufacturerNotFound);
        }

        [Fact]
        public async Task ModifyAsync_WithValidData_ShouldModifyManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerMock();
            _fixture.SetupManufacturerModification();
            _fixture.SetupModifyingManufacturer();
            _fixture.SetupFindingManufacturer();
            _fixture.SetupStoringManufacturer();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            await sut.ModifyAsync(_fixture.Modification);

            // Assert
            _fixture.VerifyModifyingManufacturer(Times.Once);
        }

        [Fact]
        public async Task ModifyAsync_WithValidData_ShouldStoreManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerMock();
            _fixture.SetupManufacturerModification();
            _fixture.SetupModifyingManufacturer();
            _fixture.SetupFindingManufacturer();
            _fixture.SetupStoringManufacturer();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            await sut.ModifyAsync(_fixture.Modification);

            // Assert
            _fixture.VerifyStoringManufacturer(Times.Once);
        }

        private sealed class ModifyAsyncFixture : LocalFixture
        {
            public ManufacturerModification? Modification { get; private set; }

            public void SetupManufacturerModification()
            {
                TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);

                Modification = new DomainTestBuilder<ManufacturerModification>()
                    .FillConstructorWith("id", ManufacturerMock.Object.Id)
                    .Create();
            }

            public void SetupModifyingManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);
                TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);
                ManufacturerMock.SetupModify(Modification);
            }

            public void VerifyModifyingManufacturer(Func<Times> times)
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);
                TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);
                ManufacturerMock.VerifyModify(Modification, times);
            }
        }
    }

    private abstract class LocalFixture
    {
        private readonly ManufacturerRepositoryMock _manufacturerRepositoryMock = new(MockBehavior.Strict);

        protected ManufacturerMock? ManufacturerMock { get; private set; }

        public ManufacturerModificationService CreateSut()
        {
            return new ManufacturerModificationService(_manufacturerRepositoryMock.Object, default);
        }

        public void SetupManufacturerMock()
        {
            var manufacturer = new ManufacturerBuilder().Create();
            ManufacturerMock = new ManufacturerMock(manufacturer);
        }

        public void SetupNotFindingManufacturer()
        {
            TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);
            _manufacturerRepositoryMock.SetupFindActiveByAsync(ManufacturerMock.Object.Id, null);
        }

        public void SetupFindingManufacturer()
        {
            TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);
            _manufacturerRepositoryMock.SetupFindActiveByAsync(ManufacturerMock.Object.Id, ManufacturerMock.Object);
        }

        public void SetupStoringManufacturer()
        {
            TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);
            _manufacturerRepositoryMock.SetupStoreAsync(ManufacturerMock.Object, ManufacturerMock.Object);
        }

        public void VerifyStoringManufacturer(Func<Times> times)
        {
            TestPropertyNotSetException.ThrowIfNull(ManufacturerMock);
            _manufacturerRepositoryMock.VerifyStoreAsync(ManufacturerMock.Object, times);
        }
    }
}