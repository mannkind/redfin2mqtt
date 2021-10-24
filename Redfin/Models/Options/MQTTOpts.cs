using TwoMQTT.Models;

namespace Redfin.Models.Options;

/// <summary>
/// The sink options
/// </summary>
public record MQTTOpts : MQTTManagerOptions
{
    public const string Section = "Redfin:MQTT";
    public const string TopicPrefixDefault = "home/redfin";
    public const string DiscoveryNameDefault = "redfin";
}
