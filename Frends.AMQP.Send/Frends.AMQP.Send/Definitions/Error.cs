using System;

namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// Detailed error information when the task fails.
/// </summary>
public class Error
{
    /// <summary>
    /// Primary error message describing what went wrong during sending message.
    /// Can be either the original exception message or a custom message if specified in options.
    /// </summary>
    /// <example>Error in message properties</example>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Additional contextual information about the error such as exception type and stack trace.
    /// Contains structured data that can be useful for automated error handling and logging.
    /// </summary>
    /// <example>{ "ExceptionType": "ArgumentException", "StackTrace": "..." }</example>
    public Exception AdditionalInfo { get; init; }
}