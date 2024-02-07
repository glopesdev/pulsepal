using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying whether trigger channel 1 can
    /// trigger the output channel.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.TriggerOnChannel1))]
    public class TriggerOnChannel1Configuration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying whether trigger channel 1 can trigger
        /// this output channel.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies whether trigger channel 1 can trigger this output channel.")]
        public bool Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetTriggerOnChannel1(Channel, Value);
        }
    }
}
