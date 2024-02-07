using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the behavior of a trigger channel.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.TriggerMode))]
    public class TriggerModeConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying the behavior of a trigger channel.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies the behavior of a trigger channel.")]
        public TriggerMode Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetTriggerMode(Channel, Value);
        }
    }
}
