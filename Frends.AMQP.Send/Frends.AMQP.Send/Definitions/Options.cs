using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// Options parameters.
/// </summary>
public class Options
{
    /// <summary>
    /// Timeout in seconds for receiving or sending Message to the queue.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int Timeout { get; set; } = 30;

    /// <summary>
    /// Link name.
    /// </summary>
    /// <example>9e2646fe-bbc7-482d-a5dc-679dc5caf951</example>
    [DefaultValue("{{Guid.NewGuid().ToString()}}")]
    [DisplayFormat(DataFormatString = "Text")]
    public string LinkName { get; set; }

    /// <summary>
    /// Determines error handling behavior. If true, throws an exception when task fails.
    /// If false, returns error information in the Result object instead of throwing.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Custom error message to use when ThrowErrorOnFailure is false and an error occurs.
    /// If null or empty, the original exception message will be used.
    /// This allows for user-friendly error messages in automated workflows.
    /// </summary>
    /// <example>Failed to send message</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string ErrorMessageOnFailure { get; set; } = "";
}