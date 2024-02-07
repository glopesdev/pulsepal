using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    [XmlType(Namespace = Constants.XmlNamespace)]
    public class ChannelParameterConfigurationCollection : Collection<ChannelParameterConfiguration>
    {
        public void Configure(PulsePal device)
        {
            foreach (var parameter in this)
            {
                parameter.Configure(device);
            }
        }
    }
}
