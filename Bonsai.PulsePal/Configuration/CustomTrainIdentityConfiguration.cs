using System.ComponentModel;
using System.Xml.Serialization;

namespace Bonsai.PulsePal.Configuration
{
    /// <summary>
    /// Represents configuration parameters specifying the identity of the custom
    /// train used to specify pulse times and voltages on an output channel.
    /// </summary>
    [XmlType(Namespace = Constants.XmlNamespace)]
    [DisplayName(nameof(ParameterCode.CustomTrainIdentity))]
    public class CustomTrainIdentityConfiguration : ChannelParameterConfiguration
    {
        /// <summary>
        /// Gets or sets a value specifying the identity of the custom pulse train
        /// to use on this output channel.
        /// </summary>
        [XmlAttribute]
        [Description("Specifies the identity of the custom pulse train to use on this output channel.")]
        public CustomTrainId Value { get; set; }

        /// <inheritdoc/>
        public override void Configure(PulsePal device)
        {
            device.SetCustomTrainIdentity(Channel, Value);
        }
    }
}
