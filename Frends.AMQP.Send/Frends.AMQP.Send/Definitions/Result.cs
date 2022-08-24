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
    public bool Success { get; private set; }

    internal Result(bool success)
    {
        Success = success;
    }
}