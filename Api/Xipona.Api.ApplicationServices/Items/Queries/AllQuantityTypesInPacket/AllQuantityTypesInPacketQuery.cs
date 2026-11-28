using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;

namespace Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;

public class AllQuantityTypesInPacketQuery : IQuery<IEnumerable<QuantityTypeInPacketReadModel>>
{
}