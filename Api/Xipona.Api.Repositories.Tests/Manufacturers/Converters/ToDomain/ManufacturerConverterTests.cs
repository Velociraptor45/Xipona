using AutoMapper;
using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Converters.ToDomain;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using Manufacturer = ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities.Manufacturer;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Manufacturers.Converters.ToDomain;

public class ManufacturerConverterTests : ToDomainConverterTestBase<Manufacturer, IManufacturer, ManufacturerConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);

    public override ManufacturerConverter CreateSut()
    {
        return new(new ManufacturerFactory(_dateTimeServiceMock.Object));
    }

    protected override void AddMapping(IMappingExpression<Manufacturer, IManufacturer> mapping)
    {
        mapping.As<Domain.Manufacturers.Models.Manufacturer>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Manufacturer, Domain.Manufacturers.Models.Manufacturer>()
            .ForCtorParam(nameof(IManufacturer.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ManufacturerId(src.Id)))
            .ForCtorParam(nameof(IManufacturer.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ManufacturerName(src.Name)))
            .ForCtorParam(nameof(IManufacturer.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(src => src.Deleted))
            .ForCtorParam(nameof(IManufacturer.CreatedAt).LowerFirstChar(), opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}