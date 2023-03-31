// See https://aka.ms/new-console-template for more information
using System.Runtime.Serialization;

namespace ConsoleApp.Exceptions;

[Serializable]
internal class DecryptionException : Exception
{
    public DecryptionException()
    {
    }

    public DecryptionException(string? message) : base(message)
    {
    }

    public DecryptionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DecryptionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}