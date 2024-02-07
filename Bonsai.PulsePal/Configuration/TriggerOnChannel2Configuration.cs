using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying whether trigger channel 2 can
    /// trigger the output channel.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.TriggerOnChannel2))]
    public class TriggerOnChannel2Configuration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying whether trigger channel 2 can trigger
        /// this output channel.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies whether trigger channel 2 can trigger this output channel.")]
        public bool Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetTriggerOnChannel2(Channel, Value);
        }
    }
}
