using System.ComponentModel.DataAnnotations;
namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// AMQP message.
/// </summary>
public class AmqpMessage
{
    /// <summary>
    /// The message body.
    /// </summary>
    /// <example>foobar!</example>
    [DisplayFormat(DataFormatString = "Text")]
    public object BodyAsString { get; set; }
}