using System;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Exceptions;

public class ApiConnectionException : Exception
{
    public ApiConnectionException(string message) : base(message)
    {
    }

    public ApiConnectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}