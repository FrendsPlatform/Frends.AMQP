using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// AMQP message string.
    /// </summary>
    /// <example>Example message.</example>
    public AmqpMessage Message { get; set; }

    /// <summary>
    /// The URI for the AMQP Message bus, username and key must be url encoded.
    /// </summary>
    /// <example>amqps://&lt;username&gt;:&lt;key&gt;@&lt;host&gt;:&lt;port&gt;</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("\"amqps://<username>:<key>@<host>:<port>\"")]
    public string BusUri { get; set; }

    /// <summary>
    /// Name of target queue or topic.
    /// </summary>
    /// <example>Queue</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string QueueOrTopicName { get; set; }
}