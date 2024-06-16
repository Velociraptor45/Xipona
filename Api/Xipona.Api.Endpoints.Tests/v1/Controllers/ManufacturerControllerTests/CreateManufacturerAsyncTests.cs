using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class CreateManufacturerAsyncTests : ControllerCommandWithReturnTypeTestsBase<ManufacturerController,
    CreateManufacturerCommand, IManufacturer, ManufacturerContract,
    CreateManufacturerAsyncTests.CreateManufacturerAsyncFixture>
{
    public CreateManufacturerAsyncTests() : base(new CreateManufacturerAsyncFixture())
    {
    }

    public sealed class CreateManufacturerAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
    {
        private string? _name;

        public CreateManufacturerAsyncFixture()
        {
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.CreateManufacturerAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return await sut.CreateManufacturerAsync(_name);
        }

        public override void SetupCommand()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            Command = new CreateManufacturerCommand(new ManufacturerName(_name));
        }

        public override void SetupParameters()
        {
            _name = new DomainTestBuilder<string>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_name, Command);
        }
    }
}