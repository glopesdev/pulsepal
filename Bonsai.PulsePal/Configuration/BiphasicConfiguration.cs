using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying whether the channel
    /// will produce either monophasic or biphasic square pulses.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.Biphasic))]
    public class BiphasicConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying whether to use biphasic or
        /// monophasic pulses.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies whether to use biphasic or monophasic pulses.")]
        public bool Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetBiphasic(Channel, Value);
        }
    }
}
