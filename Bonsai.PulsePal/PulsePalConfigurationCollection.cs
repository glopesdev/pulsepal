using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal
{
    [XmlRoot("PulsePalConfigurationSettings")]
    public class PulsePalConfigurationCollection : KeyedCollection<string, PulsePalConfiguration>
    {
        protected override string GetKeyForItem(PulsePalConfiguration item)
        {
            return item.PortName;
        }
    }
}
