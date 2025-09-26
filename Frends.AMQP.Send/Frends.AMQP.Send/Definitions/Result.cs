namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// Describes if Message was sent successfully.
/// </summary>
public class Result
{
    /// <summary>
    /// True if Message was sent successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Error information when the sending message fails and ThrowErrorOnFailure option is set to false.
    /// Contains detailed error message and additional debugging information.
    /// </summary>
    public Error Error { get; set; }
}