namespace ProjectHermes.Xipona.Api.Contracts.Common
{
    /// <summary>
    /// Represents an error containing a message and an error code.
    /// </summary>
    public class ErrorContract
    {
        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        public ErrorContract(string message, int errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The error code.
        /// </summary>
        public int ErrorCode { get; }
    }
}