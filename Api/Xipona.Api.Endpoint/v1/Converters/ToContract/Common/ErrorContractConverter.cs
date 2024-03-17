using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Common;

public class ErrorContractConverter : IToContractConverter<IReason, ErrorContract>
{
    public ErrorContract ToContract(IReason source)
    {
        return new ErrorContract(source.Message, source.ErrorCode.ToInt());
    }
}