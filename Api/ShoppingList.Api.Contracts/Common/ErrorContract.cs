namespace ProjectHermes.ShoppingList.Api.Contracts.Common
{
    public class ErrorContract
    {
        public ErrorContract(string message, int errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }

        public string Message { get; }
        public int ErrorCode { get; }
    }
}