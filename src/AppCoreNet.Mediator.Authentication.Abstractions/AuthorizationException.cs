using System;

namespace AppCoreNet.Mediator;

public class AuthorizationException : Exception
{
    public AuthorizationException()
    {
    }

    public AuthorizationException(string message)
        : base(message)
    {
    }

    public AuthorizationException(Exception innerException)
        : base("Authorization failed.", innerException)
    {
    }

    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}