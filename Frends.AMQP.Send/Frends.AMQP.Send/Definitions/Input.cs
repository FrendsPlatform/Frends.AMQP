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
    /// Name of target queue or topic.
    /// </summary>
    /// <example>Queue</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string QueueOrTopicName { get; set; }

    /// <summary>
    /// The Immutable properties of the Message.
    /// </summary>
    /// <example>AbsoluteExpiryTime, ContentEncoding, ContentType, CorrelationId, CreationTime, GroupId, GroupSequence, ReplyToGroupId, ReplyTo, Subject, UserId, To</example>
    public AmqpProperties MessageProperties { get; set; }

    /// <summary>
    /// Application properties section of an AMQP messages.
    /// </summary>
    /// <example>foo, bar</example>
    public ApplicationProperty[] ApplicationProperties { get; set; }
}