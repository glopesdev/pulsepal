using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents a collection of Pulse Pal channel configuration objects.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    public class ChannelParameterConfigurationCollection : Collection<ChannelParameterConfiguration>
    {
    }
}
