using Xipona.Api.Contracts.Common;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Reasons;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Common;

public class ErrorContractConverter : IToContractConverter<IReason, ErrorContract>
{
    public ErrorContract ToContract(IReason source)
    {
        return new ErrorContract(source.Message, source.ErrorCode.ToInt());
    }
}