namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;

public interface IQuantitiesQueryService
{
    IEnumerable<QuantityTypeReadModel> GetAllQuantityTypes();

    IEnumerable<QuantityTypeInPacketReadModel> GetAllQuantityTypesInPacket();
}