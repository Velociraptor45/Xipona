using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Common;

public class ErrorContractConverter : IToContractConverter<IReason, ErrorContract>
{
    public ErrorContract ToContract(IReason source)
    {
        return new ErrorContract(source.Message, source.ErrorCode.ToInt());
    }
}