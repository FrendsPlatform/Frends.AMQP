using System;
using Frends.AMQP.Send.Definitions;

namespace Frends.AMQP.Send.Helpers;

/// <summary>
/// Static utility class for handling errors.
/// Provides centralized error handling logic based on configuration options.
/// </summary>
public static class ErrorHandler
{
    /// <summary>
    /// Handles exceptions that occur during task execution based on the provided configuration options.
    /// Either re-throws the exception or returns a Result object with error details.
    /// </summary>
    /// <param name="exception">The exception that occurred during the process.</param>
    /// <param name="options">Configuration options that determine how errors should be handled.</param>
    /// <returns>
    /// A Result object containing error information if ThrowErrorOnFailure is false.
    /// If ThrowErrorOnFailure is true, the method re-throws the original exception.
    /// </returns>
    public static Result Handle(Exception exception, Options options)
    {
        var error = new Error
        {
            Message = string.IsNullOrWhiteSpace(options.ErrorMessageOnFailure)
                ? exception.Message
                : $"{options.ErrorMessageOnFailure.Trim()} {exception.Message}".Trim(),
            AdditionalInfo = exception,
        };

        if (options.ThrowErrorOnFailure)
            throw new Exception(error.Message, exception);

        return new Result
        {
            Success = false,
            Error = error
        };
    }
}
