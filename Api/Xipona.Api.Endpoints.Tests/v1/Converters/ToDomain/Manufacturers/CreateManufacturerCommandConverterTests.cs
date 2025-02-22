using AutoMapper;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.Manufacturers;

public class CreateManufacturerCommandConverterTests
    : ToDomainConverterTestBase<string, CreateManufacturerCommand, CreateManufacturerCommandConverter>
{
    public override CreateManufacturerCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<string, CreateManufacturerCommand> mapping)
    {
        mapping.ForCtorParam(nameof(CreateManufacturerCommand.Name).LowerFirstChar(), opt => opt.MapFrom(src => src));
    }
}