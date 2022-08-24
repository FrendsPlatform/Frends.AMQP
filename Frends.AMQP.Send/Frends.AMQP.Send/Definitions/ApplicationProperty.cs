namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// Application properties section of an AMQP messages.
/// </summary>
public class ApplicationProperty
{
    /// <summary>
    /// Application property's name.
    /// </summary>
    /// <example>foo</example>
    public string Name { get; set; }

    /// <summary>
    /// Application property's value.
    /// </summary>
    /// <example>bar</example>
    public object Value { get; set; }
}
