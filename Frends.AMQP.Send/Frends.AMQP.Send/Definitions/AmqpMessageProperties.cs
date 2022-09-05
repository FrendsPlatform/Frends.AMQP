namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// AMQP message properties.
/// </summary>
public class AmqpMessageProperties
{
    /// <summary>
    /// Application properties section of an AMQP messages.
    /// </summary>
    /// <example>foo, bar</example>
    public ApplicationProperty[] ApplicationProperties { get; set; }

    /// <summary>
    /// The Immutable properties of the Message.
    /// </summary>
    /// <example>AbsoluteExpiryTime, ContentEncoding, ContentType, CorrelationId, CreationTime, GroupId, GroupSequence, ReplyToGroupId, ReplyTo, Subject, UserId, To</example>
    public AmqpProperties Properties { get; set; }
}