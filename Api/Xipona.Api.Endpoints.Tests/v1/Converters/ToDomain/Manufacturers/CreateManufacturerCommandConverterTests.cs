using AutoMapper;
using Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.Manufacturers;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.Manufacturers;

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