using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ShoppingList.Api.TestTools.Exceptions;

public class TestPropertyNotSetException : Exception
{
    public TestPropertyNotSetException(string paramName)
        : base($"Test property {paramName} not set")
    {
    }

    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            throw new TestPropertyNotSetException(paramName ?? string.Empty);
        }
    }
}