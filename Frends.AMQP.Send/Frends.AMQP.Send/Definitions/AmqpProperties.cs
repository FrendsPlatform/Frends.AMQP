using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// The Immutable properties of the Message.
/// </summary>
public class AmqpProperties
{
    private uint _groupSequence;

    /// <summary>
    /// Message's id.
    /// </summary>
    /// <example>43c8c21c-f1db-41d6-be68-5b476b24e46d</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("Guid.NewGuid().ToString()")]
    public string MessageId { get; set; }

    /// <summary>
    /// Message's AbsoluteExpiryTime.
    /// </summary>
    /// <example>1.1.0001 0.00.00</example>
    public DateTime? AbsoluteExpiryTime { get; set; }

    /// <summary>
    /// Message's ContentEncoding.
    /// </summary>
    /// <example>UTF8</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ContentEncoding { get; set; }

    /// <summary>
    /// Message's ContentType.
    /// </summary>
    /// <example>text/plain</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ContentType { get; set; }

    /// <summary>
    /// Message's CorrelationId.
    /// </summary>
    /// <example>43c8c21c-f1db-41d6-be68-5b476b24e46d</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string CorrelationId { get; set; }

    /// <summary>
    /// Message's CreationTime.
    /// </summary>
    /// <example>24.8.2022 12.00.00</example>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// Message's GroupId.
    /// </summary>
    /// <example>0</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string GroupId { get; set; }

    /// <summary>
    /// Message's GroupSequence.
    /// </summary>
    /// <example>0</example>
    [DefaultValue(0)]
    public uint GroupSequence { get; set; }

    /// <summary>
    /// Message's ReplyToGroupId.
    /// </summary>
    /// <example>0</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ReplyToGroupId { get; set; }

    /// <summary>
    /// Message's ReplyTo.
    /// </summary>
    /// <example>foo</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string ReplyTo { get; set; }

    /// <summary>
    /// Message's Subject.
    /// </summary>
    /// <example>foo bar</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string Subject { get; set; }

    /// <summary>
    /// Message's UserId.
    /// </summary>
    /// <example>0</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public byte[] UserId { get; set; }

    /// <summary>
    /// Message's To.
    /// </summary>
    /// <example>foo</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("")]
    public string To { get; set; }
}